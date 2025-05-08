//---------------------------------------------------------
// Se encarga de gestionar la barra de sueño de los animales
// Sergio Valiente Urueña, Sergio González López
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// La clase BarraDeSueño se encarga de gestionar la barra de sueño de los animales.
/// </summary>
public class BarraDeSueño : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("Atributos del Inspector")]
    [SerializeField] float MaxBarraDeSueño;
    [SerializeField] private float ResetDistance;

    [Header("Colliders")]
    //Distintos colliders para el animal dependiendo de su estado.
    [SerializeField] Collider2D NormalCollider;
    [SerializeField] Collider2D TriggerCollider;
    [SerializeField] Collider2D SleepCollider;

    [SerializeField] Collider2D SleepLeftCollider;
    [SerializeField] Collider2D SleepRightCollider;
    
    [Header("UI Elements")]
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private float _fillSpeed;
    [SerializeField] private Gradient _colorGradient;

    [Header("Referencias")]
    [SerializeField] private Transform Player;
    
    [Header("Sistema de guardado")]
    [SerializeField] int _animalId;


    //Para diferenciar si el animal es macho o hembra.
    [SerializeField] private bool male;
    public bool Male
    {
        get => male;
    }

    //Para el sistema de guardado
    public int AnimalId
    {
        get => _animalId;
    }

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float _barraDeSueño = 0; 
    private AnimalController _animalController;
    private Animator _animator;
    private bool dormido;
    private bool _playerDetected;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Awake()
    {
        _animalController = GetComponent<AnimalController>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (_playerDetected && !dormido) 
        {
            ResetBar();
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método que se llama desde el script Bullet para aumentar el sueño del animal.
    /// </summary>
    public void Dormir(float amount)
    {
        if(!dormido)
        {
            _barraDeSueño += amount;
            _playerDetected = true;
            AudioManager.Instance.PlaySFX("animalHurt", true);

            if(_barraDeSueño >= MaxBarraDeSueño)
            {
                _animalController.HideDangerSignal();
                
                if (_animalController.IsFlipped)
                {
                    _animator.SetTrigger("Sleep_Right");

                    if(SleepRightCollider != null)
                    SleepRightCollider.enabled = true;
                }
                else 
                {
                    _animator.SetTrigger("Sleep_Left");

                    if(SleepLeftCollider != null)
                    SleepLeftCollider.enabled = true;
                }
                AudioManager.Instance.PlaySFX("animalKO");

                _animalController.enabled = false;

                if(NormalCollider != null)
                {
                    NormalCollider.enabled = false;
                }
                if(SleepCollider != null)
                {
                    SleepCollider.enabled = true;
                }

                dormido = true;
            }

            _barraDeSueño = Mathf.Clamp(_barraDeSueño, 0f, MaxBarraDeSueño);
            UpdateHealthBar();
        }
    }

    public bool Dormido ()
    { return dormido; }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Actualiza la barra de sueño del animal.
    /// </summary>
    private void UpdateHealthBar()
    {
        float targetFillAmount = _barraDeSueño / MaxBarraDeSueño;
        _healthBarFill.DOFillAmount(targetFillAmount, _fillSpeed);
        _healthBarFill.color = _colorGradient.Evaluate(targetFillAmount);
    }

    /// <summary>
    /// Resetea la barra de sueño del animal si el jugador se aleja demasiado.
    /// </summary>
    private void ResetBar()
    {
        float _distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        
        if (_distanceToPlayer > ResetDistance)
        {
            _barraDeSueño = 0;
            UpdateHealthBar();
            _playerDetected = false;
        }
    }
    #endregion   

} // class BarraDeSueño 
// namespace
