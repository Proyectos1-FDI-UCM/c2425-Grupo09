//---------------------------------------------------------
// Contiene lo necesario para guardar la partida
// Diego García
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.IO;
// Añadir aquí el resto de directivas using
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SaveSystem
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    private static SaveData _saveData = new SaveData();
    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData PlayerData;
        public PlayerAbilitiesData AbilitiesData;
        public TimerData timerData;
        public HealthData healthData;
        public CaptureData captureData;
        public ItemData itemData;
    }
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public static string SaveFileName()
    {
        string _saveFile = Application.persistentDataPath + "/save" + ".save";
        return _saveFile;
    }

    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    public static void Load()
    {
        string _saveContent = File.ReadAllText(SaveFileName());
        _saveData = JsonUtility.FromJson<SaveData>( _saveContent );
        HandleLoadData();
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private static void HandleSaveData()
    {
        GameManager.Instance._PlayerController.Save(ref _saveData.PlayerData);
        GameManager.Instance._AbilitiesManager.Save(ref _saveData.AbilitiesData);
        GameManager.Instance._Timer.Save(ref _saveData.timerData);
        GameManager.Instance._Health.Save(ref _saveData.healthData);
        GameManager.Instance._Capture.Save(ref _saveData.captureData);
        GameManager.Instance._InventoryController.Save(ref _saveData.itemData);

    }
    private static void HandleLoadData()
    {
        GameManager.Instance._PlayerController.Load( _saveData.PlayerData);
        GameManager.Instance._AbilitiesManager.Load( _saveData.AbilitiesData);
        GameManager.Instance._Timer.Load( _saveData.timerData);
        GameManager.Instance._Health.Load(_saveData.healthData);
        GameManager.Instance._Capture.Load( _saveData.captureData);
        GameManager.Instance._InventoryController.Load(_saveData.itemData);
        GameManager.Instance._PlayerController.Abilities();
        GameManager.Instance._Health.Abilities();
        GameManager.Instance._Gun.Abilities();
    }
    #endregion   

} // class Save 
// namespace
