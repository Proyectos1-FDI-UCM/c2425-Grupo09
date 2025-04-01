//---------------------------------------------------------
// Script responsable de detectar si un objeto toca el agua
// y aplicar la mecánica de muerte utilizando Duck Typing.
// Sergio González López
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Clase Water que define el comportamiento del agua en el juego.
/// Cualquier objeto que colisione con ella y tenga el método EsAgua() 
/// será considerado agua.
/// </summary>
public class Water : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    // No hay atributos visibles en el inspector necesarios para esta funcionalidad.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    // No se necesitan atributos privados en este caso.

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start se llama antes de la primera actualización del frame.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Método que se ejecuta cuando otro objeto entra en el Trigger de este objeto.
    /// Usa Duck Typing para verificar si el objeto tiene un método "EsAgua".
    /// </summary>
    /// <param name="other">El Collider del objeto que entra en contacto con el agua.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Health>() != null) 
        
        {
            other.gameObject.GetComponent<Health>().Updatehealth(-200f);
        }
            
        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

   
   

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

 
  

    #endregion
} // class Water
