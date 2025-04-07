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

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Animator _fadeAnim;
    private bool _isPaused = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (InputManager.Instance.PauseWasPressedThisFrame())
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    private void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _fadeAnim = Fade.GetComponent<Animator>();
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

    private IEnumerator EffectAnimation(int index)
    {
        ObtainEffect.SetActive(true);
        //AudioManager.Instance.PlaySFX(AudioManager.Instance.magicCharge);
        yield return new WaitForSeconds(1.8f);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.magicExplosion);
        CameraShakeManager.Instance.StandardCameraShake();
        yield return new WaitForSeconds(0.2f);
        EnableAbilityTextBox(index, true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.magicTinkle);
        yield return new WaitForSeconds(2f);
        ObtainEffect.SetActive(false);
        yield return new WaitForSeconds(2f);
        EnableAbilityTextBox(index, false);
    }

    private void EnableAbilityTextBox(int index, bool state)
    {
        AbilitiesTextBox[index].SetActive(state);
    }
    #endregion

} // class UIManager 
// namespace
