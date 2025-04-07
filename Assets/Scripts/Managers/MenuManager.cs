//---------------------------------------------------------
// Script que controla las opciones del menú principal.
// Valeria Espada
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MenuManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private RectTransform titlePanel;
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] TMP_Text playText; 
    

    

    public float speed = 0.1f;  // Velocidad del parpadeo
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Color color1 = new Color(0.176f, 0.110f, 0f); 
    private Color color2 = Color.white; 
    private float time = 0f;
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        StartCoroutine(SlideInTitlePanel());
        StartCoroutine(SlideInMenuPanel());
        AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenu);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.PlayWasPressedThisFrame())
        { SceneManager.LoadScene("FUSION_MAPA"); }

        // Interpolación entre los dos colores según el tiempo
        time += Time.deltaTime * speed;

        // Usamos Mathf.PingPong para alternar entre 0 y 1
        float t = Mathf.PingPong(time, 1f);

        // Lerp entre los dos colores
        playText.color = Color.Lerp(color1, color2, t);
    
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private IEnumerator SlideInTitlePanel()
    {
        // Pos original
        Vector2 originalPosition = titlePanel.anchoredPosition;

        // Pos inicial
        titlePanel.anchoredPosition = new Vector2(originalPosition.x, Screen.height);

        // Velocidad y duración de movimiento
        float moveDuration = 1f;
        float elapsedTime = 0f;

        // Animación
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;

            float currentY = Mathf.Lerp(Screen.height, originalPosition.y, t);
            titlePanel.anchoredPosition = new Vector2(originalPosition.x, currentY);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        titlePanel.anchoredPosition = originalPosition;
    }

    private IEnumerator SlideInMenuPanel()
    {
        // Pos original
        Vector2 originalPosition = menuPanel.anchoredPosition;

        // Pos inicial
        menuPanel.anchoredPosition = new Vector2(originalPosition.x, - Screen.height);

        // Velocidad y duración de movimiento
        float moveDuration = 1f;
        float elapsedTime = 0f;

        // Animación
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;

            float currentY = Mathf.Lerp(- Screen.height, originalPosition.y, t);
            menuPanel.anchoredPosition = new Vector2(originalPosition.x, currentY);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        menuPanel.anchoredPosition = originalPosition;
    }

    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class MenuManager 
// namespace


