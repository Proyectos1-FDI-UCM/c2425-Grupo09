//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Sergio Valiente Urueña, Sergio Gonzalez
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
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
    [SerializeField] GameObject[] AbilitiesTextBoxGamePad;

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
        _health = GameManager.Instance._health;
    }
    private void Update()
    {
        if (InputManager.Instance.PauseWasPressedThisFrame() && !_isPaused)
        {
            PauseGame();
            InputManager.Instance.EnableUIControls();
        }
        if (InputManager.Instance.PauseMenuCloseWasPressedThisFrame())
        {
            ResumeGame();
            InputManager.Instance.EnablePlayerControls();
        }
    }
    private void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }


    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void FadeIn()
    {
        _fadeAnim.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        _fadeAnim.SetTrigger("FadeOut");
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void VFXObtainAbility(int index)
    {
        StartCoroutine(EffectAnimation(index));
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Corrutine para gestionar lo que ocurre cuando se obtiene una habilidad.
    /// </summary>
    /// <param name="index">Indice de la habilidad.</param>
    /// <returns></returns>
    private IEnumerator EffectAnimation(int index)
    {
        ObtainEffect.SetActive(true);
        _health.DisablePlayerForAbilityVFX();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.magicCharge);

        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.magicExplosion);
        CameraShakeManager.Instance.StandardCameraShake();

        yield return new WaitForSeconds(0.2f);

        EnableAbilityTextBox(index, true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.magicTinkle);
        _health.EnablePlayerForAbilityVFX();

        yield return new WaitForSeconds(2f);

        ObtainEffect.SetActive(false);

        yield return new WaitForSeconds(2f);
        EnableAbilityTextBox(index, false);
    }

    private void EnableAbilityTextBox(int index, bool state)
    {
        if(InputManager.Instance.MandoConectado())
        {
            if(AbilitiesTextBoxGamePad[index] != null)
                AbilitiesTextBoxGamePad[index].SetActive(state);
            else 
                AbilitiesTextBox[index].SetActive(state);

        }else{

            AbilitiesTextBox[index].SetActive(state);
        }
    }
    #endregion

} // class UIManager 
// namespace
