//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Sergio Valiente Urueña
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
    [SerializeField] private GameObject GameOverMenu;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Animator _fadeAnim;

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
        Fade.SetActive(true);
    }

    public void FadeOut()
    {
        StartCoroutine(FadeDelay());
    }

    public void ShowGameOverMenu()
    {
        GameOverMenu.SetActive(true);
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private IEnumerator FadeDelay()
    {
        _fadeAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.4f);
        Fade.SetActive(false);
    }

    #endregion   

} // class UIManager 
// namespace
