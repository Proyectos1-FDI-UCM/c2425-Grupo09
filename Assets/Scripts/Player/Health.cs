//---------------------------------------------------------
// Mecanica responsable de manejar la vida del jugador(incluyendo el escudo), es decir, de Noe.
// Alejandro García Díaz, Sergio González, Diego García y Sergio Valiente
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using Cinemachine;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
// Añadir aquí el resto de directivas using

/// <summary>
/// Este script sera el responsable de manejar la vida(y el escudo) del jugador,
/// aumentarla si se cura y reducirla si recibe daño
/// </summary>
public class Health : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("Vida")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float startingHealth = 100f;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private float _fillSpeed;
    [SerializeField] private Gradient _colorGradient;
    [SerializeField] Animator animator;

    [Header("Escudo")]
    [SerializeField] float ShieldDuration;
    [SerializeField] float Shield;
    [SerializeField] float CooldownShield;
    [SerializeField] private Image _shieldBarFill; // Barra de escudo

    public float CurrentHealth
    {
        get => _currentHealth;
    }

    public bool ArmadilloUnlocked
    {
        get => armadilloUnlocked;
        set => armadilloUnlocked = value;
    }

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float _timeLastShield;
    private float _currentDuration;
    private float _dañoActual;
    private float _currentShield;
    private float _shieldDown;
    private bool _shieldOn;
    private float _currentHealth;
    private bool armadilloUnlocked = false;

    private PlayerController _playerController;
    private bool playerDisabledForAbilityVFX = false;
    private CinemachineImpulseSource _cinemachineImpulseSource;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Awake()
    {
        if(GetComponent<CinemachineImpulseSource>() != null)
        {
            _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        } 
        else Debug.Log("CinemachineImpulseSource no encontrado");
        

        if(GetComponent<PlayerController>() != null)
        {
            _playerController = GetComponent<PlayerController>();
        } 
        else Debug.Log("PlayerController no encontrado");
    }

    void Start()
    {
        Abilities();
        _currentDuration = ShieldDuration;
        _currentHealth = startingHealth;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
        _shieldOn = false;
        UpdateHealthBar();
        

        CheckpointManager.Instance.PlayerReference(gameObject);
    }

    private void Update()
    {
        if (!armadilloUnlocked)
        {
            HUDAbilities.Instance.SetArmadilloIconState(HUDAbilities.ArmadilloState.Locked);
            return;
        }
        if (_currentShield > 0)
        {
                // ESCUDO ACTIVO
                _currentDuration -= Time.deltaTime;
                _shieldOn = true;


            HUDAbilities.Instance.SetArmadilloIconState(HUDAbilities.ArmadilloState.Active);
                HUDAbilities.Instance.UpdateCountDown(_currentDuration);

                if (_currentDuration <= 0)
                {
                    _currentShield = 0;
                    _shieldDown = CooldownShield;
                    UpdateShieldBar();
                    UpdateHealthBar();
                }
            
            
        }
        else if (_shieldDown > 0)
        {
            // COOLDOWN
            _shieldOn = false;
            _shieldDown -= Time.deltaTime;

            HUDAbilities.Instance.SetArmadilloIconState(HUDAbilities.ArmadilloState.Cooldown);
            HUDAbilities.Instance.UpdateCountDown(_shieldDown);
        }
        else
        {
            // DISPONIBLE
            _shieldOn = false;
            HUDAbilities.Instance.SetArmadilloIconState(HUDAbilities.ArmadilloState.Ready);
            HUDAbilities.Instance.HideCountDown();
        }

        if (InputManager.Instance.ShieldWasPressedThisFrame() && Time.time > _timeLastShield + CooldownShield && _shieldDown <= 0 && armadilloUnlocked && !_shieldOn)
        {
            _timeLastShield = Time.time;
            OnShield();
            AudioManager.Instance.PlaySFX("shield", true);
            _currentDuration = ShieldDuration;
        }

    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Se usa para actualizar la vida del jugador, cuando come una manzana
    /// o cuando le ataca un animal.
    /// </summary>
    public void Updatehealth(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
        _dañoActual = amount;

        if (_dañoActual < 0) 
        {
            CameraShakeManager.Instance.CameraShake(_cinemachineImpulseSource, 1f);
        }

        if (-amount > _currentShield)
        {
            _dañoActual += _currentShield;
            _currentShield = 0;
            _currentHealth += _dañoActual;
            UpdateShieldBar();
        }
        else if (_currentShield > 0 && amount < 0)
        {

            _currentShield += amount;
            UpdateShieldBar();
        }
        else if (_currentShield <= 0 )
        {
            _currentHealth += amount;
        }
        else if (amount > 0)
        {
            _currentHealth += amount;
            UpdateShieldBar();
        }

        UpdateHealthBar();

        if (_currentHealth <= 0) //Si la vida llega a 0 muere
        {
           Die();
        } else if(amount < 0) {
            animator.SetTrigger("Hurt"); 
            AudioManager.Instance.PlaySFX("playerHurt", true);
            EnablePlayerForAnimation();
        }
    }
    /// <summary>
    /// Se usa cuando el jugador muere.
    /// </summary>
    public void Die()
    {
        animator.SetTrigger("Dead");
        AudioManager.Instance.PlaySFX("die", false);
        GameManager.Instance._Gun?.ReleaseGrapple();
        _playerController.DisablePlayer();
        CheckpointManager.Instance.Revivir();
    }
    /// <summary>
    /// Se llama cuando el jugador respawnea.
    /// </summary>
    public void ResetPlayer()
    {
        _playerController.enabled = true;
        _currentHealth = 100f;
        Updatehealth(0f);
    }

    /// <summary>
    /// Se activa el movimiento del jugador. Se llama desde eventos de animación.
    /// </summary>
    public void EnablePlayerForAnimation()
    {
        if(!playerDisabledForAbilityVFX)
        _playerController.enabled = true;
    }

    /// <summary>
    /// Se desactiva el movimiento del jugador. Se llama desde eventos de animación.
    /// </summary>
    public void DisablePlayerForAnimation()
    {
        _playerController.enabled = false;
    }

    /// <summary>
    /// Se desactiva el movimiento del jugador. Se llama desde el UIManager.
    /// </summary>
    public void DisablePlayerForAbilityVFX()
    {
        _playerController.enabled = false;
        animator.SetBool("Walk", false);
        playerDisabledForAbilityVFX = true;
    }

    /// <summary>
    /// Se activa el movimiento del jugador. Se llama desde el UIManager.
    /// </summary>
    public void EnablePlayerForAbilityVFX()
    {
        _playerController.enabled = true;
        playerDisabledForAbilityVFX = false;
    }

    #region Save and Load
    /// <summary>
    /// Se usa para guardar la cantidad de vida del jugador.
    /// </summary>
    public void Save(ref HealthData data)
    {
        data.healthAmount = _currentHealth;

    }
    /// <summary>
    /// Se usa para cargar la cantidad de vida del jugador.
    /// </summary>
    public void Load( HealthData data)
    {
        _currentHealth = data.healthAmount;
        UpdateHealthBar();
        UpdateShieldBar();
    }
    #endregion
    /// <summary>
    /// Se usa para otorgar la habilidad del armadillo.
    /// </summary>
    public void Abilities()
    {
        if (AbilitiesManager.Instance.Armadillo == true) { armadilloUnlocked = true; }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos privados
    /// <summary>
    /// Este método actualiza la barra de vida del jugador
    /// </summary>
    private void UpdateHealthBar()
    {
        float maximoRelativo = _currentHealth + _currentShield > 100 ? _currentHealth + _currentShield : maxHealth;

        float targetFillAmount = _currentHealth / maximoRelativo;
        _healthBarFill.DOFillAmount(targetFillAmount, _fillSpeed);
        _healthBarFill.color = _colorGradient.Evaluate(_currentHealth / maxHealth);
    }

    /// <summary>
    /// Este método actualiza la barra de escudo del jugador
    /// </summary>
    private void UpdateShieldBar()
    {
        if (Shield > 0) // Asegurar que no haya división por cero
        {
            float maximoRelativo = _currentHealth + _currentShield > 100 ? _currentHealth + _currentShield : maxHealth;

            float targetFillAmount = (_currentShield + _currentHealth) / maximoRelativo;
            _shieldBarFill.DOFillAmount(targetFillAmount, _fillSpeed);

            if (_currentShield <= 0)
            {
                HUDAbilities.Instance.ArmadilloGlow(false);
            }
        }
    }
    /// <summary>
    /// Este método pone el escudo al jugador
    /// </summary>
    private void OnShield()
    {
        HUDAbilities.Instance.ArmadilloGlow(true);
        _currentShield += Shield;
        _currentDuration = ShieldDuration; // Reinicia la duración del escudo
        UpdateShieldBar(); // Asegurar que la barra del escudo se llene visualmente
        UpdateHealthBar();
    }
    #endregion

} // class Health 
  // namespace
#region Struct SaveAndLoad
/// <summary>
/// Guarda la vida del jugador
/// </summary>
[System.Serializable]
public struct HealthData
{
    public float healthAmount;
}
#endregion