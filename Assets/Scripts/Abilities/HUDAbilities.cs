//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Pablo Abellán
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.UI;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class HUDAbilities : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    public static HUDAbilities Instance { get; private set; }

    [SerializeField] Image[] Greyimage;
    [SerializeField] Sprite[] Colorimage;
    [SerializeField] GameObject[] ColorKeysImage;
    [SerializeField] GameObject[] ColorKeysImageGamePad;
    [SerializeField] GameObject[] GlowEffect;

    
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private enum _animal
    {
        Armadillo,
        Bat,
        Gorilla
    }

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

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
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
        
    }

    public void GorillaGlow(bool state)
    {
        GlowEffect[(int)_animal.Gorilla].SetActive(state); 
    }

    public void BatGlow(bool state)
    {
        GlowEffect[(int)_animal.Bat].SetActive(state);
    }

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
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class HUD-Abilities 
// namespace
