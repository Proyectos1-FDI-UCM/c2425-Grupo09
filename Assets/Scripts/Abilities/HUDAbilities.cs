//---------------------------------------------------------
// El script se encarga de gestionar el HUD de habilidades del juego, mostrando los iconos y su estado (bloqueado, activo, en espera, etc.).
// Pablo Abellán, Sergio Valiente
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controla el HUD de habilidades. Cambia el estado visual del icono de habilidades (color, brillo, cuenta atrás)
/// según el estado de activación y desbloqueo.
/// </summary>
public class HUDAbilities : MonoBehaviour
{
    // ---- SINGLETON ----
    public static HUDAbilities Instance { get; private set; }

    public enum ArmadilloState
    {
        Locked,     // No se ha obtenido: icono gris
        Ready,      // Obtenida pero no usada: icono en color sin brillo
        Active,     // Escudo activo: icono en color con brillo
        Cooldown    // En espera para volver a usar: icono en color sin brillo + cuenta atrás
    }


    // ---- ATRIBUTOS DEL INSPECTOR ----
    [Header("Iconos de habilidad")]
    // Iconos en gris
    [SerializeField] private Image[] Greyimage;       
    // Iconos en color
    [SerializeField] private Sprite[] Colorimage;     
    //Iconos de la tecla que hay que pulsar para activar la habilidad
    [SerializeField] private GameObject[] ColorKeysImage;
    //Efecto de brillo del icono
    [SerializeField] private GameObject[] GlowEffect;
    //Boton de la habilidad en PS4
    [SerializeField] GameObject[] ColorKeysImagePS4;
    //Boton de la habilidad en Xbox
    [SerializeField] GameObject[] ColorKeysImageXbox;
    //Icono del control que hay que pulsar para consumir una manzana (0: teclado, 1: mando PS4, 2: mando Xbox)
    [SerializeField] GameObject[] ApplesControls;


    [Header("Texto de cuenta atrás")]
    [SerializeField] private TMP_Text ArmadilloText;

    // ---- ENUM PRIVADO PARA INDEXACIÓN ----
    private enum _animal
    {
        Armadillo,
        Bat,
        Gorilla
    }

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    protected void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ---- MÉTODOS PÚBLICOS ----

    /// <summary>
    /// Cambia el icono a color y muestra el key bind (cuando se desbloquea).
    /// </summary>
    public void ActivateColor(int index)
    {
        Greyimage[index].sprite = Colorimage[index];

        if(InputManager.Instance.GetDevice() == InputManager.Dispositivo.PS4)
        {
            if (ColorKeysImagePS4[index] != null) 
             ColorKeysImagePS4[index].SetActive(true);
        }
        else if(InputManager.Instance.GetDevice() == InputManager.Dispositivo.XBOX)
        {
            if (ColorKeysImageXbox[index] != null) 
             ColorKeysImageXbox[index].SetActive(true);
        }
        else
        {
            if (ColorKeysImage[index] != null) 
             ColorKeysImage[index].SetActive(true);
        }
        
       
    }

    /// <summary>
    /// Controla el brillo del gorila.
    /// </summary>
    public void GorillaGlow(bool state)
    {
        GlowEffect[(int)_animal.Gorilla].SetActive(state);
    }

    /// <summary>
    /// Controla el brillo del murciélago.
    /// </summary>
    public void BatGlow(bool state)
    {
        GlowEffect[(int)_animal.Bat].SetActive(state);
    }

    /// <summary>
    /// Controla el brillo del armadillo.
    /// </summary>
    public void ArmadilloGlow(bool state)
    {
        GlowEffect[(int)_animal.Armadillo].SetActive(state);
    }

    
    /// <summary>
    /// Cambia el icono del control a mando o teclado.
    /// </summary>
    /// <param name="mando">Si es true, el icono es del mando, si es false, el icono es del teclado.</param>
    public void UpdateHUDForGamePad(InputManager.Dispositivo _dispositivo)
    {
        if(_dispositivo == InputManager.Dispositivo.PS4)
        {
            for(int i = 0; i < ColorKeysImage.Length; i++)
            {
                if(ColorKeysImage[i] != null && ColorKeysImage[i].activeSelf == true)
                {
                    ColorKeysImage[i].SetActive(false);
                    ColorKeysImagePS4[i].SetActive(true);
                    ColorKeysImageXbox[i].SetActive(false);
                }
            }

            ApplesControls[0].SetActive(false);
            ApplesControls[1].SetActive(true);
            ApplesControls[2].SetActive(false);
        }
        else if (_dispositivo == InputManager.Dispositivo.Teclado)
        {
            for(int i = 0; i < ColorKeysImagePS4.Length; i++)
            {
                if(ColorKeysImage[i] != null && ColorKeysImagePS4[i].activeSelf == true)
                {
                    ColorKeysImage[i].SetActive(true);
                    ColorKeysImagePS4[i].SetActive(false);
                    ColorKeysImageXbox[i].SetActive(false);
                }
            }

            ApplesControls[0].SetActive(true);
            ApplesControls[1].SetActive(false);
            ApplesControls[2].SetActive(false);
        }
        else if (_dispositivo == InputManager.Dispositivo.XBOX)
        {
            for (int i = 0; i < ColorKeysImage.Length; i++)
            {
                if (ColorKeysImage[i] != null && ColorKeysImage[i].activeSelf == true)
                {
                    ColorKeysImage[i].SetActive(false);
                    ColorKeysImagePS4[i].SetActive(false);
                    ColorKeysImageXbox[i].SetActive(true);
                }
            }

            ApplesControls[0].SetActive(false);
            ApplesControls[1].SetActive(false);
            ApplesControls[2].SetActive(true);
        }
        
    }

    /// <summary>
    /// Actualiza la cuenta atrás que se muestra en el HUD.
    /// </summary>
    public void UpdateCountDown(float seconds)
    {
        int s = Mathf.FloorToInt(seconds);
        if (s >= 0)
        {
            ArmadilloText.gameObject.SetActive(true);
            ArmadilloText.text = s.ToString();
        }
        else
        {
            ArmadilloText.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// Oculta el texto de cuenta regresiva.
    /// </summary>
    public void HideCountDown()
    {
        ArmadilloText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Cambia el estado visual del icono del armadillo en función del estado del escudo.
    /// </summary>
    public void SetArmadilloIconState(ArmadilloState state)
    {
        switch (state)
        {
            case ArmadilloState.Locked:
                GlowEffect[(int)_animal.Armadillo].SetActive(false);
                ArmadilloText.gameObject.SetActive(false);
                break;

            case ArmadilloState.Ready:
                Greyimage[(int)_animal.Armadillo].color = Color.white;
                GlowEffect[(int)_animal.Armadillo].SetActive(false);
                ArmadilloText.gameObject.SetActive(false);
                break;

            case ArmadilloState.Active:
                Greyimage[(int)_animal.Armadillo].color = Color.white;
                GlowEffect[(int)_animal.Armadillo].SetActive(true);
                ArmadilloText.color = Color.black;

                break;

            case ArmadilloState.Cooldown:
                Greyimage[(int)_animal.Armadillo].color = Color.gray;
                GlowEffect[(int)_animal.Armadillo].SetActive(false);
                ArmadilloText.color = Color.white;
                break;
        }
    }
}

