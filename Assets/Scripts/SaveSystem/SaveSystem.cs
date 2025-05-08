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

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Este método da el nombre al archivo en el que se guarda y carga todo
    /// </summary>
    public static string SaveFileName()
    {
        string _saveFile = Application.persistentDataPath + "/save" + ".save";
        return _saveFile;
    }
    /// <summary>
    /// Este método guarda las cosas en el archivo
    /// </summary>
    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }
    /// <summary>
    /// Este método carga lo que recibe del archivo
    /// </summary>
    public static void Load()
    {
        string _saveContent = File.ReadAllText(SaveFileName());
        _saveData = JsonUtility.FromJson<SaveData>( _saveContent );
        HandleLoadData();
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Este método gestiona todos los datos guardados
    /// </summary>
    private static void HandleSaveData()
    {
        GameManager.Instance._PlayerController.Save(ref _saveData.PlayerData);
        GameManager.Instance._AbilitiesManager.Save(ref _saveData.AbilitiesData);
        GameManager.Instance._Timer.Save(ref _saveData.timerData);
        GameManager.Instance._Health.Save(ref _saveData.healthData);
        GameManager.Instance._Capture.Save(ref _saveData.captureData);
        GameManager.Instance._InventoryController.Save(ref _saveData.itemData);

    }
    /// <summary>
    /// Este método gestiona todos los datos cargados
    /// </summary>
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
