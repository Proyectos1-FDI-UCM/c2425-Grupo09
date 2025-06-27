//---------------------------------------------------------
// Script encargado de aumentar el daño al consumir el coco
// Alejandro Garcia Diaz
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using static UnityEditor.Progress;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Coconut : MonoBehaviour
{
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private PlayerController _playerController;

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerController = collision.GetComponent<PlayerController>();

        if (collision.gameObject.GetComponent<Health>() != null)
        {
            _playerController.CoconutUnlocked = true;
            ConsumableManager.Instance.CoconutConsumed();
            AudioManager.Instance.PlaySFX("pickApple");
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    #endregion   

} // class Coconut 
// namespace
