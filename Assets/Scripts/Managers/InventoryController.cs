//---------------------------------------------------------
// Responsable de manejar todo el contenido que muestra el inventario en pantalla (numero de manzanas, etc...)
// Alejandro García Díaz
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class InventoryController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] TextMeshProUGUI numApples;
    [SerializeField] GameObject grayGoldenApple;
    [SerializeField] private int appleHealthUp = 50;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Health _health;
    
    private int aviableApples;
    private int applesInInventory = 0;

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
    void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Update()
    {
        if (InputManager.Instance.HealWasPressedThisFrame() && applesInInventory > 0 && _health._currentHealth < 100) //Curacion al consumir una manzana
        {
            applesInInventory--;
            _health.Updatehealth(appleHealthUp);
            AppleConsumed();
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public void AddAppleToInventory()
    {
        applesInInventory++;
        NumAppleToText(applesInInventory);
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
   
    private void AppleConsumed()
    {
        aviableApples--;
        NumAppleToText(aviableApples);
    }

    private void NumAppleToText(int applesInInventory)
    {
        if (applesInInventory > 0)
        {
            aviableApples = applesInInventory;
            grayGoldenApple.SetActive(false);
            numApples.gameObject.SetActive(true);
            numApples.text = Convert.ToString(aviableApples);
        }
        else
        {
            numApples.gameObject.SetActive(false);
            grayGoldenApple.SetActive(true);
        }
    }

    #endregion   

} // class InventoryController 
// namespace
