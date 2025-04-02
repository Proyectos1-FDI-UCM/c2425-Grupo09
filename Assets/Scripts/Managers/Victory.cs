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
    void Start()
    {
        this.gameObject.SetActive(false);
        _capture = player.GetComponent<Capture>();
        _timer = timer.GetComponent<Timer>();
    }

    public void ShowVictory()
    {
        _animals = _capture.AnimalCount();
        _minutes = _timer.MinutesCount();
        _seconds = _timer.SecondsCount();
        _timer.StopCounting();
        this.gameObject.SetActive(true);

        AnimalsText.text = "Animales capturados: " + _animals.ToString();
        TimeText.text = "Tiempo de partida: " + string.Format("{0:00}:{1:00}", _minutes, _seconds);

        // Llamamos a la animación para mostrar el cartel con el deslizamiento
        StartCoroutine(SlideInPanel());
        MenuButton.onClick.AddListener(OnButtonClick);
    }

    // Corutina que anima el cartel desde arriba y lo estabiliza en el centro
    private IEnumerator SlideInPanel()
    {
        // Guarda la posición original del panel
        Vector2 originalPosition = victoryPanel.anchoredPosition;

        // Inicializa el panel fuera de la vista (arriba de la pantalla)
        victoryPanel.anchoredPosition = new Vector2(originalPosition.x, Screen.height);

        // Velocidad de movimiento
        float moveDuration = 0.7f; // Duración total de la animación
        float elapsedTime = 0f;

        // Realiza la animación: baja hasta la posición final
        while (elapsedTime < moveDuration)
        {
            // Proporción del tiempo transcurrido (t)
            float t = elapsedTime / moveDuration;

            // Mueve el panel de arriba hacia la posición final
            float currentY = Mathf.Lerp(Screen.height, originalPosition.y, t);
            victoryPanel.anchoredPosition = new Vector2(originalPosition.x, currentY);

            // Incrementa el tiempo transcurrido
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Asegura que el panel esté exactamente en la posición original
        victoryPanel.anchoredPosition = originalPosition;
    }



    #endregion



    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

    }

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
    private void OnButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

        #endregion
} // class Victory 
// namespace
