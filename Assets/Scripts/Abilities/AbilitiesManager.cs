//---------------------------------------------------------
// Script responsable de manejar cuando pueden funcionar las habilidades
// Alejandro García Díaz, Pablo Abellán, Diego García Alonso
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
// Añadir aquí el resto de directivas using

public class AbilitiesManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    public static AbilitiesManager Instance { get; private set; }

    [SerializeField] private bool nightVision = false;
    [SerializeField] private bool doubleJump = false;
    [SerializeField] private bool grappler = false;
    [SerializeField] private bool tiger = false;
    [SerializeField] private bool armadillo = false;

    public bool NightVision { get { return nightVision; } }
    public bool DoubleJump { get { return doubleJump; } }
    public bool Grappler { get { return grappler; } }
    public bool Tiger { get { return tiger; } }
    public bool Armadillo { get { return armadillo; } }

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
    /// <summary>
    /// Se llama cuando el jugador captura el primer conejo
    /// para otorgarle su habilidad
    /// </summary>
    public void BunnyAbilityUnlock()
    {
        if(!doubleJump)
        {
            doubleJump = true;
            UIManager.Instance.VFXObtainAbility((int)_HUDImage.Bunny);
        }
    }
    /// <summary>
    /// Se llama cuando el jugador captura el primer murciélago
    /// para otorgarle su habilidad
    /// </summary>
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
    /// <summary>
    /// Se llama cuando el jugador captura el primer gorila
    /// para otorgarle su habilidad
    /// </summary>
    public void GorilaAbilityUnlock()
    {
        if(!grappler)
        {
            grappler = true;
            _HUDAbilities.ActivateColor((int)_HUDImage.Gorila);
            UIManager.Instance.VFXObtainAbility((int)_HUDImage.Gorila);
        }
       
    }
    /// <summary>
    /// Se llama cuando el jugador captura el primer tigre
    /// para otorgarle su habilidad
    /// </summary>
    public void TigerAbilityUnlock()
    {
        if(!tiger)
        {
            tiger = true;
            UIManager.Instance.VFXObtainAbility((int)_HUDImage.Tiger);
        }
        
    }
    /// <summary>
    /// Se llama cuando el jugador captura el primer armadillo
    /// para otorgarle su habilidad
    /// </summary>
    public void ArmadilloAbilityUnlock()
    {
        if(!armadillo)
        {
            armadillo = true;
            _HUDAbilities.ActivateColor((int)_HUDImage.Armadillo);
            UIManager.Instance.VFXObtainAbility((int)_HUDImage.Armadillo);
        }
    }
    /// <summary>
    /// Se usa para otorgar todas las habilidades al jugador,
    /// al activarse los cheats
    /// </summary>
    public void Cheats()
    {
        doubleJump = true;
        nightVision = true;
        grappler = true;
        tiger = true;
        armadillo = true;
        
        for (int i = 0; i < _caveTriggers.Length; i++)
        {
            if (_caveTriggers[i] != null)
                _caveTriggers[i].UnlockCave();
        }

        _HUDAbilities.ActivateColor((int)_HUDImage.Bat);
        _HUDAbilities.ActivateColor((int)_HUDImage.Gorila);
        _HUDAbilities.ActivateColor((int)_HUDImage.Armadillo);
    }

    #region Save and Load
    /// <summary>
    /// Se usa para guardar las habilidades que tiene el jugador 
    /// </summary>
    public void Save(ref PlayerAbilitiesData data)
    {
        data.nightVision = nightVision;
        data.doubleJump = doubleJump;
        data.grappler = grappler;
        data.tiger = tiger;
        data.armadillo = armadillo;
    }
    /// <summary>
    /// Se usa para cargar las habilidades que tiene el jugador 
    /// </summary>
    public void Load(PlayerAbilitiesData data) 
    { 
        nightVision = data.nightVision;
        doubleJump = data.doubleJump;
        grappler = data.grappler;
        tiger = data.tiger;
        armadillo = data.armadillo;
        if(nightVision) _HUDAbilities.ActivateColor((int)_HUDImage.Bat);
        if(grappler) _HUDAbilities.ActivateColor((int)_HUDImage.Gorila);
        if (armadillo) _HUDAbilities.ActivateColor((int)_HUDImage.Armadillo);
    }

    #endregion

    #endregion

} // class AbilitiesController 
  // namespace
#region Struct Save
/// <summary>
/// Guarda las habilidades que ya ha conseguido el jugador
/// </summary>
[System.Serializable]
public struct PlayerAbilitiesData
{
    public bool nightVision;
    public bool doubleJump;
    public bool grappler;
    public bool tiger;
    public bool armadillo;
}
#endregion