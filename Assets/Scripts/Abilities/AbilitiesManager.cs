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

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    HUDAbilities _HUDAbilities;
    CaveEntrance[] _caveTriggers;

    enum _HUDImage
    {
        Armadillo,
        Bat,
        Gorila,
        Tiger,
        Bunny
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
            UIManager.Instance.VFXObtainAbility((int)_HUDImage.Bunny);
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
            UIManager.Instance.VFXObtainAbility((int)_HUDImage.Bat);
        }
        

    }
    public void GorilaAbilityUnlock()
    {
        if(!grappler)
        {
            grappler = true;
            _HUDAbilities.ActivateColor((int)_HUDImage.Gorila);
            UIManager.Instance.VFXObtainAbility((int)_HUDImage.Gorila);
        }
       
    }
    public void TigerAbilityUnlock()
    {
        if(!tiger)
        {
            tiger = true;
            UIManager.Instance.VFXObtainAbility((int)_HUDImage.Tiger);
        }
        
    }
    public void ArmadilloAbilityUnlock()
    {
        if(!armadillo)
        {
            armadillo = true;
            _HUDAbilities.ActivateColor((int)_HUDImage.Armadillo);
            UIManager.Instance.VFXObtainAbility((int)_HUDImage.Armadillo);
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    
    #endregion

} // class AbilitiesController 
// namespace
