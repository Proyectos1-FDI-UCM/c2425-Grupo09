//---------------------------------------------------------
// Script usado para el desbloque de las zonas del mapa cuando el jugador pasa por un trigger
// Diego García
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.Tilemaps;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class RoomRevealer : MonoBehaviour
{
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Detecta si el jugador ha tocado el trigger para destruirlo
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Destroy(gameObject);
        }
    }

    #endregion   

} // class RoomRevealer 
// namespace
