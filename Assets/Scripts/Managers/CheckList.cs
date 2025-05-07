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
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.InputSystem.Controls;


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
    [SerializeField] private TextMeshProUGUI TutorialVesselText;
    [SerializeField] private TextMeshProUGUI PS4VesselText;
    [SerializeField] private TextMeshProUGUI XBOXVesselText;
    [SerializeField] private TextMeshProUGUI KeyboardVesselText;


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
    private bool _inKeyboard;
    private Victory _victory;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    [SerializeField] private bool _onVessel;
    public bool OnVessel
    {
        get => _onVessel;
    }

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
        TutorialVesselText.gameObject.SetActive(false);
        PS4VesselText.gameObject.SetActive(false);
        KeyboardVesselText.gameObject.SetActive(false);
        XBOXVesselText.gameObject.SetActive(false);

        //_allCaptured = true;
        _onVessel = false;
        _victory = victorySign.GetComponent<Victory>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_onVessel)
        {
            TutorialVesselText.gameObject.SetActive(true);

            if (InputManager.Instance.GetDevice() == InputManager.Dispositivo.PS4)
            {
                PS4VesselText.gameObject.SetActive(true);
                KeyboardVesselText.gameObject.SetActive(false);
                XBOXVesselText.gameObject.SetActive(false);
            }
            else if (InputManager.Instance.GetDevice() == InputManager.Dispositivo.XBOX)
            {
                PS4VesselText.gameObject.SetActive(false);
                XBOXVesselText.gameObject.SetActive(true);
                KeyboardVesselText.gameObject.SetActive(false);
            }
            else
            {
                KeyboardVesselText.gameObject.SetActive(true);
                XBOXVesselText.gameObject.SetActive(false);
                PS4VesselText.gameObject.SetActive(false);
            }

        }
        else
        {
            TutorialVesselText.gameObject.SetActive(false);
            PS4VesselText.gameObject.SetActive(false);
            KeyboardVesselText.gameObject.SetActive(false);
        }

        if (InputManager.Instance.ChecklistWasPressedThisFrame()) // Detecta la tecla Tab
        {
            ToggleMenu();
            InputManager.Instance.EnableUIControls();
        }
        else if (InputManager.Instance.ChecklistCLoseWasPressedThisFrame() && _isMenuOpen)
        {
            ToggleMenu();
            InputManager.Instance.EnablePlayerControls();
        }
        if (_onVessel && !_allCaptured && InputManager.Instance.ExitWasPressedThisFrame())
        {
            VesselText.gameObject.SetActive(true);
            StartCoroutine(DeactivateVesselTextAfterTime(2f));
        }
        else if (_onVessel && _allCaptured && InputManager.Instance.ExitWasPressedThisFrame())
        {
            _victory.ShowVictory(); 
            InputManager.Instance.DisablePlayerControls();
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
    }
    public void ToggleMenu()
    {
        if (menuPanel != null)
        {
            AudioManager.Instance.PlaySFX("checklist", true);
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

    public void Cheats()
    {
        for (int i = 0; i < ticks.Length; i++)
        {
            ActivateTick(i);
        }
    }
    #endregion

} // class CheckList 
