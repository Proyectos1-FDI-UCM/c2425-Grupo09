//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Sergio Valiente Urueña, Sergio Gonzalez López , Diego García Alonso
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


/// <summary>
/// Clase que gestiona la interfaz de usuario del juego.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject Fade;
    [SerializeField] private GameObject PauseMenu;

    [SerializeField] private GameObject ObtainEffect;
    [SerializeField] GameObject[] AbilitiesTextBox;
    [SerializeField] GameObject[] AbilitiesTextBoxPS4;
    [SerializeField] GameObject[] AbilitiesTextBoxXbox;
    [SerializeField] GameObject FirstButton;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Animator _fadeAnim;
    private Health _health;
    private bool _isPaused = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        _fadeAnim = Fade.GetComponent<Animator>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _health = GameManager.Instance._Health;
    }
    private void Update()
    {
        if (InputManager.Instance.PauseWasPressedThisFrame() && !_isPaused) PauseGame();
        else if (InputManager.Instance.PauseMenuCloseWasPressedThisFrame() ) ResumeGame();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Se llama cuando se abre cualquier menu, 
    /// para hacer una pequeña transición de entrada.
    /// </summary>
    public void FadeIn()
    {
        _fadeAnim.SetTrigger("FadeIn");
    }
    /// <summary>
    /// Se llama cuando se abre cualquier menu , 
    /// para hacer una pequeña transición de salida.
    /// </summary>
    public void FadeOut()
    {
        _fadeAnim.SetTrigger("FadeOut");
    }
    /// <summary>
    /// Se llama cuando se cierra el menu de pausa,
    /// para reanudar la partida.
    /// </summary>
    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
        InputManager.Instance.EnablePlayerControls();
    }
    /// <summary>
    /// Se llama cuando el jugador obtiene una habilidad nueva.
    /// </summary>
    public void VFXObtainAbility(int index)
    {
        StartCoroutine(EffectAnimation(index));
    }
    /// <summary>
    /// Al clickar el boton de Main Menu vuelve a la escena de main menu.
    /// </summary>
    public void OnButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Se llama cuando se abre el menu de pausa.
    /// </summary>
    private void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
        InputManager.Instance.EnableUIControls();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(FirstButton);
    }

    /// <summary>
    /// Corrutine para gestionar lo que ocurre cuando se obtiene una habilidad.
    /// </summary>
    /// <param name="index">Indice de la habilidad.</param>
    /// <returns></returns>
    private IEnumerator EffectAnimation(int index)
    {
        ObtainEffect.SetActive(true);
        _health.DisablePlayerForAbilityVFX();
        AudioManager.Instance.PlaySFX("magicCharge");

        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlaySFX("magicExplosion");
        CameraShakeManager.Instance.StandardCameraShake();

        yield return new WaitForSeconds(0.2f);

        EnableAbilityTextBox(index, true);
        AudioManager.Instance.PlaySFX("magicTinkle");
        _health.EnablePlayerForAbilityVFX();

        yield return new WaitForSeconds(2f);

        ObtainEffect.SetActive(false);

        yield return new WaitForSeconds(2f);
        EnableAbilityTextBox(index, false);
    }
    /// <summary>
    /// Se llama cuando el jugador consigue una habilidad nueva, 
    /// le muestra un pequeño mensaje.
    /// </summary>
    private void EnableAbilityTextBox(int index, bool state)
    {
        if(InputManager.Instance.GetDevice() == InputManager.Dispositivo.PS4)
        {
            if(AbilitiesTextBoxPS4[index] != null)
                AbilitiesTextBoxPS4[index].SetActive(state);
            else 
                AbilitiesTextBox[index].SetActive(state);

        }else if(InputManager.Instance.GetDevice() == InputManager.Dispositivo.XBOX){

            if(AbilitiesTextBoxXbox[index] != null)
                AbilitiesTextBoxXbox[index].SetActive(state);
            else 
                AbilitiesTextBox[index].SetActive(state);
        }else 
        {
            AbilitiesTextBox[index].SetActive(state);
        }
    }


    #endregion

} // class UIManager 
// namespace
