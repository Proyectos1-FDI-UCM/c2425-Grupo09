//---------------------------------------------------------
// Script que se encarga de que el efecto visual siga al jugador
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

public class ObteinAbilityVFX : MonoBehaviour
{    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private GameObject _player;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Obtenemos la referencia al jugador al iniciar el juego.
    /// </summary>
    void Awake()
    {
        _player = FindFirstObjectByType<PlayerController>().gameObject;
    }

    /// <summary>
    /// Actualiza la posición del efecto visual para que siga al jugador.
    /// </summary>
    void Update()
    {
        transform.position = _player.transform.position;
    }
    #endregion

} // class ObteinAbilityVFX 
// namespace
