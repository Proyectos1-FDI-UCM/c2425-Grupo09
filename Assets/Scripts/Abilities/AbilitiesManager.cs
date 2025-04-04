//---------------------------------------------------------
// Script responsable de manejar cuando pueden funcionar las habilidades
// Alejandro García Díaz, Pablo Abellán
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
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
        doubleJump = true;
        ObtainEffect.SetActive(true);
    }
    public void BatAbilityUnlock()
    {
        nightVision = true;

        for(int i = 0; i < _caveTriggers.Length; i++)
        {
            if (_caveTriggers[i] != null)
            _caveTriggers[i].UnlockCave();
        }
        
        _HUDAbilities.ActivateColor((int)_HUDImage.Bat);
    }
    public void GorilaAbilityUnlock()
    {
        grappler = true;
        _HUDAbilities.ActivateColor((int)_HUDImage.Gorila);
    }
    public void TigerAbilityUnlock()
    {
        tiger = true;
    }
    public void ArmadilloAbilityUnlock()
    {
        armadillo = true;
        _HUDAbilities.ActivateColor((int)_HUDImage.Armadillo);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion

} // class AbilitiesController 
// namespace
