//---------------------------------------------------------
// Script responsable de todos los cambios al consumir las semillas doradas
// Alejandro García Díaz
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Seed : MonoBehaviour
{
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private InventoryController _inventoryController;

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _inventoryController = collision.GetComponent<InventoryController>();

        if (collision.gameObject.GetComponent<Health>() != null)
        {
            _inventoryController.appleHealthUp = 35;
            ConsumableManager.Instance.SeedConsumed();
            AudioManager.Instance.PlaySFX("pickApple");
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    #endregion   

} // class Seed 
// namespace
