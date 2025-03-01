//---------------------------------------------------------
// Script que se encarga de gestionar el disparo
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

/// <summary>
/// Script que se encarga de gestionar el disparo
/// </summary>
public class Weapon : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
   
    //Prefab de la bala
    [SerializeField] private GameObject BulletPrefab;
    //Cadencia de disparo
    [SerializeField] private float CadenciaDisparo;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    //Variable para almacenar el último tiempo en el que se disparó
    private float _tiempoUltimoDisparo;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Si se detecta el input del disparo, y ha pasado el tiempo de cooldown desde el disparo anterior, ejecuta la accion de disparar.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.FireWasPressedThisFrame() && Time.time > _tiempoUltimoDisparo + CadenciaDisparo)
        {
            Shoot();
            _tiempoUltimoDisparo = Time.time;
        }
            
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    
    /// <summary>
    /// Instancia la bala en la posición del jugador 
    /// </summary>
    private void Shoot()
    {
       Instantiate(BulletPrefab, transform.position, transform.rotation);
    }



    #endregion   

} // class Shoot 
// namespace
