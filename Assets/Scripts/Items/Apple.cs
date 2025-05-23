//---------------------------------------------------------
// Script hecho para detectar si el jugador ha colisionado con una manzana
// Sergio Valiente Urueña, Diego García Alonso
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Apple : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Se usa para guardar que manzana ha sido destruida y cual no
    /// </summary>
    [SerializeField] int ItemId;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private InventoryController _inventoryController;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _inventoryController = FindFirstObjectByType<InventoryController>();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Se usa para saber si el jugador ha colisionado con la manzana
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Health>() != null)
        {
            _inventoryController.InventoryAdd(ItemId);
            AudioManager.Instance.PlaySFX("pickApple");
            gameObject.SetActive(false);
            _inventoryController.AddAppleToInventory();
            Destroy(gameObject);
        }
    }

    #endregion   

} // class Apple 
// namespace
