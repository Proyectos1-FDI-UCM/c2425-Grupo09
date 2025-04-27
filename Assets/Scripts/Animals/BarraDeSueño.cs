//---------------------------------------------------------
// Se encarga de gestionar la barra de sueño de los animales
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class BarraDeSueño : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] float MaxBarraDeSueño;

    [SerializeField] Collider2D NormalCollider;
    [SerializeField] Collider2D TriggerCollider;
    [SerializeField] Collider2D SleepCollider;

    [SerializeField] Collider2D SleepLeftCollider;
    [SerializeField] Collider2D SleepRightCollider;
    
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private float _fillSpeed;
    [SerializeField] private Gradient _colorGradient;
    [SerializeField] int _animalId;

    public bool Male;
    public int AnimalId
    {
        get => _animalId;
    }

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    [SerializeField] private float _barraDeSueño = 0; 
    //De momento está SerializeField para poder comprobar en el inspector que aumenta correctamente. Luego se quitará.
    private AnimalController _animalController;
    private Animator _animator;
    private bool dormido;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Awake()
    {
        _animalController = GetComponent<AnimalController>();
        _animator = GetComponent<Animator>();
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
            AudioManager.Instance.PlaySFX(AudioManager.Instance.animalHurt, true);

            if(_barraDeSueño >= MaxBarraDeSueño)
            {
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
                AudioManager.Instance.PlaySFX(AudioManager.Instance.animalKO);

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
    private void UpdateHealthBar()
    {
        float targetFillAmount = _barraDeSueño / MaxBarraDeSueño;
        _healthBarFill.DOFillAmount(targetFillAmount, _fillSpeed);
        _healthBarFill.color = _colorGradient.Evaluate(targetFillAmount);
    }

    #endregion   

} // class BarraDeSueño 
// namespace
