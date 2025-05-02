//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] GameObject PopUp;
    [SerializeField] GameObject[] ControlsText;
    [SerializeField] GameObject[] ControlsTextGamepad;

    [SerializeField] TextMeshProUGUI TextComponent;
    [SerializeField] float TextSpeed;
    [SerializeField] float initialWaitTime;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private int popUpIndex;
    private int controlsPopUpIndex;
    private float waitTime; 

    private string[] lines = {
    "UWU",
    "The Great Flood is coming...\nRelentless rain will soon drown the world.\nOnly you can gather the animals and lead them to safety.",    
    "You can view the list of rescued animals by pressing         , and use the map for guidance by pressing \n     .",
    "Save as many as you can before the waters rise!",
    };

    private Timer _timer;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

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
            if(waitTime <= 0)
            {
                PopUp.SetActive(true);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
                NextLine();
            }else waitTime -= Time.deltaTime;
            
        }
        else if(popUpIndex == 1){

            if(InputManager.Instance.NextTutorialWasPressedThisFrame()){
                PopUp.SetActive(false);
                PopUp.SetActive(true);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
                NextLine();
            } 

        }else if(popUpIndex == 2){
            
            if(InputManager.Instance.NextTutorialWasPressedThisFrame())
            {
                PopUp.SetActive(false);
                PopUp.SetActive(true);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
                NextLine();
            }

        } else if(popUpIndex == 3){

            if(InputManager.Instance.NextTutorialWasPressedThisFrame())
            {
                PopUp.SetActive(false);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
                _timer.StartCounting();
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
            StartCoroutine(TypeLine());
        }
    }

    #endregion   

} // class TutorialManager 
// namespace
