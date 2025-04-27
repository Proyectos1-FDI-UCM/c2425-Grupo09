//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Pablo Abellán
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
    [SerializeField] private Image[] Greyimage;       // Iconos de fondo
    [SerializeField] private Sprite[] Colorimage;     // Iconos en color
    [SerializeField] private GameObject[] ColorKeysImage;
    [SerializeField] private GameObject[] GlowEffect;
    [SerializeField] GameObject[] ColorKeysImageGamePad;


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

        if(InputManager.Instance.MandoConectado())
        {
            if (ColorKeysImageGamePad[index] != null) 
             ColorKeysImageGamePad[index].SetActive(true);
        }
        else
        {
            if (ColorKeysImage[index] != null) 
             ColorKeysImage[index].SetActive(true);
        }
        
        if (ColorKeysImage[index] != null)
            ColorKeysImage[index].SetActive(true);
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

    public void UpdateHUDForGamePad(bool mando)
    {
        if(mando)
        {
            for(int i = 0; i < ColorKeysImage.Length; i++)
            {
                if(ColorKeysImage[i] != null && ColorKeysImage[i].activeSelf == true)
                {
                    ColorKeysImage[i].SetActive(false);
                    ColorKeysImageGamePad[i].SetActive(true);
                }
            }
        }
        else
        {
            for(int i = 0; i < ColorKeysImageGamePad.Length; i++)
            {
                if(ColorKeysImage[i] != null && ColorKeysImageGamePad[i].activeSelf == true)
                {
                    ColorKeysImage[i].SetActive(true);
                    ColorKeysImageGamePad[i].SetActive(false);
                }
            }
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
                Greyimage[(int)_animal.Armadillo].color = Color.gray;
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

