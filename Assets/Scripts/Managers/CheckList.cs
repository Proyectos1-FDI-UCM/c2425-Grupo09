//---------------------------------------------------------
// En este script se gestiona el menu de la checklist de animales mediante el tabulador
// Pablo Abellán
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CheckList : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private GameObject menuPanel; // Aqui referenciamos a el panel del menú de checklist
    [SerializeField] private GameObject[] ticks;
    [SerializeField] private TextMeshProUGUI VesselText;
    [SerializeField] private GameObject VesselTextBack;
    [SerializeField] private GameObject victorySign;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private bool _isMenuOpen = false;
    private int _ticks = 0;
    private bool _allCaptured;
    private bool _inVesselRange;
    private Victory _victory;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    public bool _onVessel;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false); // Con esto hacemos que el menú esté oculto al inicio
        }

        VesselText.gameObject.SetActive(false);
        VesselTextBack.gameObject.SetActive(false);
        _allCaptured = true;
        _onVessel = false;
        _victory = victorySign.GetComponent<Victory>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.ChecklistWasPressedThisFrame()) // Detecta la tecla Tab
        {
            ToggleMenu();
        }
        if (_onVessel && !_allCaptured && InputManager.Instance.ExitWasPressedThisFrame())
        {
            VesselText.gameObject.SetActive(true);
            VesselTextBack.gameObject.SetActive(true);
            StartCoroutine(DeactivateVesselTextAfterTime(2f));
        }
        else if (_onVessel && _allCaptured && InputManager.Instance.ExitWasPressedThisFrame())
        {
            _victory.ShowVictory(); 
        }
        if (_ticks==10)
        {
            _allCaptured=true;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Activa o desactiva el menú de checklist al presionar Tab.
    /// </summary>
    IEnumerator DeactivateVesselTextAfterTime(float waitTime)
    {
        // Espera el tiempo especificado
        yield return new WaitForSeconds(waitTime);

        // Desactiva el texto después del tiempo de espera
        VesselText.gameObject.SetActive(false);
        VesselTextBack.gameObject.SetActive(false);
    }
    public void ToggleMenu()
    {
        if (menuPanel != null)
        {
            _isMenuOpen = !_isMenuOpen;
            menuPanel.SetActive(_isMenuOpen);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Vessel"))
        {
            _onVessel = true;
            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Vessel"))
        {
            _onVessel = false;
        }
    }
    public void ActivateTick(int _index)
    {
        ticks[_index].SetActive(true);
        _ticks++;
    } //Activa dentro de ticks
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    #endregion
} // class CheckList 
