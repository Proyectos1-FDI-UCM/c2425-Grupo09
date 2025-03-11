//---------------------------------------------------------
// Mecanica responsable de manejar la vida del jugador(incluyendo el escudo), es decir, de Noe.
// Alejandro García Díaz, Sergio González y Diego García
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
// Añadir aquí el resto de directivas using

/// <summary>
/// Este script sera el responsable de manejar la vida(y el escudo) del jugador,
/// aumentarla si se cura y reducirla si recibe daño
/// </summary>
public class Health : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] float maxHealth = 100f;
    [SerializeField] float startingHealth = 100f;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private float _fillSpeed;
    [SerializeField] private Gradient _colorGradient;
    [SerializeField] Animator animator;
    [HideInInspector] public float _currentHealth;
    [SerializeField] float ShieldDuration;
    [SerializeField] float Shield;
    [SerializeField] float CooldownShield;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float _timeLastShield;
    private float _currentDuration;
    private float _dañoActual;
    private float _currentShield;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start()
    {
        _currentDuration = ShieldDuration;
        _currentHealth = startingHealth;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
        UpdateHealthBar();
    }

    private void Update()
    {
        _currentDuration -= Time.deltaTime;

        if (InputManager.Instance.ShieldWasPressedThisFrame() && Time.time > _timeLastShield + CooldownShield)
        {
            _timeLastShield = Time.time;
            OnShield();
            _currentDuration = ShieldDuration;

        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public void Updatehealth(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
        _dañoActual = amount;
        if (-amount > _currentShield)
        {
            _dañoActual += _currentShield;
            _currentShield = 0;
            _currentHealth += _dañoActual;
        }
        else if (_currentShield > 0)
            _currentShield += amount;
        else if (_currentShield <= 0)
        {
            _currentHealth += amount;
        }

        UpdateHealthBar();

        if (_currentHealth <= 0) //Si la vida llega a 0 se destruye
        {
            Debug.Log("Muerto");
            //gameObject.SetActive(false);
            animator.SetTrigger("Dead");
            //Destroy(gameObject);
        }
    }
    public void HurtAnimation()
    { animator.SetTrigger("Hurt"); }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos privados
    private void UpdateHealthBar()
    {
        float targetFillAmount = _currentHealth / maxHealth;
        _healthBarFill.DOFillAmount(targetFillAmount, _fillSpeed);
        _healthBarFill.color = _colorGradient.Evaluate(targetFillAmount);
    }
    /// <summary>
    /// Este método pone el escudo al jugador
    /// </summary>
    private void OnShield()
    {
        _currentShield = Shield;
    }
    #endregion

} // class Health 
// namespace
