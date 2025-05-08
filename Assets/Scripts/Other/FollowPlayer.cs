//---------------------------------------------------------
// Script para que un objeto siga al jugador (se usa en el AudioListener)
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class FollowPlayer : MonoBehaviour
{

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private GameObject _player;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    void Start()
    {
        _player = FindFirstObjectByType<PlayerController>().gameObject;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        transform.position = _player.transform.position;
    }
    #endregion
} // class FollowPlayer 
// namespace
