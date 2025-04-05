//---------------------------------------------------------
// Script responsable de manejar cuando pueden funcionar las habilidades
// Alejandro García Díaz, Pablo Abellán
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class AbilitiesManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    public static AbilitiesManager Instance { get; private set; }

    public bool nightVision = false;
    public bool doubleJump = false;
    public bool grappler = false;
    public bool tiger = false;
    public bool armadillo = false;

    [SerializeField] private GameObject ObtainEffect;
    [SerializeField] GameObject[] AbilitiesTextBox;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    HUDAbilities _HUDAbilities;
    CaveEntrance[] _caveTriggers;

    enum _HUDImage
    {
        Armadillo,
        Bat,
        Gorila
    }
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _HUDAbilities = GetComponent<HUDAbilities>();
        _caveTriggers = FindObjectsOfType<CaveEntrance>();
    }

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

    public void BunnyAbilityUnlock()
    {
        if(!doubleJump)
        {
            doubleJump = true;
            StartCoroutine(EffectAnimation(0));
        }
    }

    public void BatAbilityUnlock()
    {
        if(!nightVision)
        {
            nightVision = true;

            for(int i = 0; i < _caveTriggers.Length; i++)
            {
                if (_caveTriggers[i] != null)
                _caveTriggers[i].UnlockCave();
            }
            
            _HUDAbilities.ActivateColor((int)_HUDImage.Bat);
            StartCoroutine(EffectAnimation(1));
        }
        

    }
    public void GorilaAbilityUnlock()
    {
        if(!grappler)
        {
            grappler = true;
            _HUDAbilities.ActivateColor((int)_HUDImage.Gorila);
            StartCoroutine(EffectAnimation(2));
        }
       
    }
    public void TigerAbilityUnlock()
    {
        if(!tiger)
        {
            tiger = true;
            StartCoroutine(EffectAnimation(3));
        }
        
    }
    public void ArmadilloAbilityUnlock()
    {
        if(!armadillo)
        {
            armadillo = true;
            _HUDAbilities.ActivateColor((int)_HUDImage.Armadillo);
            StartCoroutine(EffectAnimation(4));
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private IEnumerator EffectAnimation(int index)
    {
        ObtainEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        EnableAbilityTextBox(index, true);
        yield return new WaitForSeconds(2f);
        ObtainEffect.SetActive(false);
        yield return new WaitForSeconds(2f);
        EnableAbilityTextBox(index, false);
    }

     public void EnableAbilityTextBox(int index, bool state)
    {
        AbilitiesTextBox[index].SetActive(state);
    }
    #endregion

} // class AbilitiesController 
// namespace
