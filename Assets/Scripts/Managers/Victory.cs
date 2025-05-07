//---------------------------------------------------------
// Manager del cartel de victoria
// Valeria Espada
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private TextMeshProUGUI AnimalsText;
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject player;
    [SerializeField] private RectTransform victoryPanel;
    [SerializeField] private Button MenuButton;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Capture _capture;
    private Timer _timer;
    private int _animals;
    private float _minutes;
    private float _seconds;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// El método Start oculta el cartel y asigna los componentes Capture y Timer.
    /// </summary>
    void Start()
    {
        this.gameObject.SetActive(false);
        _capture = player.GetComponent<Capture>();
        _timer = timer.GetComponent<Timer>();
    }
    /// <summary>
    /// Método que muestra el cartel de victoria cuando el jugador escapa. Escribe los textos AnimalCount y TimerCount con las estadísticas de la partida. Activa el botón del menú.
    /// </summary>
    public void ShowVictory()
    {
        _animals = _capture.AnimalCount();
        _minutes = _timer.MinutesCount();
        _seconds = _timer.SecondsCount();
        _timer.StopCounting();
        this.gameObject.SetActive(true);

        AnimalsText.text = "Captured animals: " + _animals.ToString();
        TimeText.text = "Time: " + string.Format("{0:00}:{1:00}", _minutes, _seconds);

        // Animación cartel
        StartCoroutine(SlideInPanel());
        AudioManager.Instance.PlayMusic("victory");
        MenuButton.onClick.AddListener(OnButtonClick);
    }

    /// <summary>
    /// Método que hace la animación del cartel cuando se desliza desde lo alto de la pantalla hasta el centro.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SlideInPanel()
    {
        // Pos original
        Vector2 originalPosition = victoryPanel.anchoredPosition;

        // Pos inicial 
        victoryPanel.anchoredPosition = new Vector2(originalPosition.x, Screen.height);

        // Velocidad y duración de movimiento
        float moveDuration = 0.7f; 
        float elapsedTime = 0f;

        // Animación
        while (elapsedTime < moveDuration)
        {
            
            float t = elapsedTime / moveDuration;

            
            float currentY = Mathf.Lerp(Screen.height, originalPosition.y, t);
            victoryPanel.anchoredPosition = new Vector2(originalPosition.x, currentY);

            
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        victoryPanel.anchoredPosition = originalPosition;
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
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Vuelve a la escena menú al presionar el botón.
    /// </summary>
    private void OnButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

        #endregion
} // class Victory 
// namespace
