//---------------------------------------------------------
// Este script se encarga de gestionar el tutorial inicial del juego.
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    //La ventana emergente de los tutoriales
    [SerializeField] GameObject PopUp;
    //El texto relativo a los controles de teclado
    [SerializeField] GameObject[] ControlsText;
    //El texto relativo a los controles de mando
    [SerializeField] GameObject[] ControlsTextGamepad;

    //El texto que se va a mostrar en la ventana emergente
    [SerializeField] TextMeshProUGUI TextComponent;
    //Velocidad de escritura del texto
    [SerializeField] float TextSpeed;
    //Tiempo de espera antes de mostrar el primer popUp
    [SerializeField] float initialWaitTime;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    //Indice para llevar la cuenta del texto que se está mostrando
    private int popUpIndex;
    //Indice para llevar la cuenta del texto de los controles que se están mostrando
    private int controlsPopUpIndex;
    private float waitTime; 

    //Array de textos que se van a mostrar en la ventana emergente
    private string[] lines = {
    " ",
    "The Great Flood is coming...\nRelentless rain will soon drown the world.\nOnly you can gather the animals and lead them to safety.",    
    "You can view the list of rescued animals by pressing         , and use the map for guidance by pressing \n     .",
    "Save as many as you can before the waters rise!",
    };

    //Referencia al temporizador para controlar el temporizador
    private Timer _timer;
    //Referencia a la corrutina que escribe el texto
    private Coroutine _typeLine;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Paramos el temporizador, activamos el fade out y activamos los controles de UI.
    /// </summary>
    void Start()
    {
        _timer = FindFirstObjectByType<Timer>();
        _timer.StopCounting();

        UIManager.Instance.FadeOut();
        TextComponent.text = string.Empty;
        waitTime = initialWaitTime;
        InputManager.Instance.EnableUIControls();
    }

    private void Update()
    {
        if(popUpIndex == 0)
        {
            //Esperamos un tiempo antes de mostrar el primer popUp
            if(waitTime <= 0)
            {
                PopUp.SetActive(true);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
                NextLine();
            }else waitTime -= Time.deltaTime;
            
        }
        else if(popUpIndex == 1){

            if(InputManager.Instance.NextTutorialWasPressedThisFrame()){

                //Paramos la corrutina anterior para que no se quede escribiendo el texto en caso de que el jugador pulse antes de tiempo
                StopCoroutine(_typeLine);
                
                //Desactivamos y activamos el popUp para que se vea la animación de entrada otra vez
                PopUp.SetActive(false);
                PopUp.SetActive(true);

                //Reproducimos el sonido de click
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);

                //Pasamos a la siguiente línea
                NextLine();
            } 

        }else if(popUpIndex == 2){
            
            if(InputManager.Instance.NextTutorialWasPressedThisFrame())
            {
                StopCoroutine(_typeLine);

                PopUp.SetActive(false);
                PopUp.SetActive(true);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
                NextLine();
            }

        } else if(popUpIndex == 3){

            if(InputManager.Instance.NextTutorialWasPressedThisFrame())
            {
                StopCoroutine(_typeLine);

                PopUp.SetActive(false);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);

                //Reanudamos el temporizador y activamos los controles del jugador
                _timer.StartCounting();
                InputManager.Instance.EnablePlayerControls();
                popUpIndex++;
            }
            
        } 
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Esta función se encarga de escribir el texto del array lines en la posición popUpIndex en la pantalla letra a letra.
    /// Si detecta la palabra "pressing" activa el texto de los controles correspondientes (mando o teclado).
    /// </summary>
    /// <returns></returns>
    private IEnumerator TypeLine()
    {
        string currentWord = "";
        foreach(char c in lines[popUpIndex].ToCharArray())
        {
            TextComponent.text += c;

            if(c != ' ')
                currentWord += c;
            else 
                currentWord = string.Empty;

            if(currentWord == "pressing")
            {
                if(InputManager.Instance.MandoConectado())
                    ControlsTextGamepad[controlsPopUpIndex].SetActive(true);
                else 
                    ControlsText[controlsPopUpIndex].SetActive(true);

                controlsPopUpIndex++;
            }            

            yield return new WaitForSecondsRealtime(TextSpeed);
        }
    }

    /// <summary>
    /// Esta función se encarga de ocultar el texto de los controles y de pasar a la siguiente línea del array lines.
    /// </summary>
    private void NextLine()
    {
        for(int i = 0; i < ControlsText.Length; i++)
        {
            ControlsText[i].SetActive(false);
            ControlsTextGamepad[i].SetActive(false);
        }

        if(popUpIndex < lines.Length - 1)
        {
            popUpIndex++;
            TextComponent.text = string.Empty;
            _typeLine = StartCoroutine(TypeLine());
        }
    }

    #endregion   

} // class TutorialManager 
// namespace
