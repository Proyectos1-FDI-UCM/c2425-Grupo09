//---------------------------------------------------------
// Este script se encarga de gestionar el sistema de Parallax. Lo lleva la main Camera.
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
 
/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate OnCameraTranslate;

    #endregion
 
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float _oldPosition;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
 
    /// <summary>
    /// Guarda la posición inicial de la cámara en el eje X.
    /// </summary>

    void Start()
    {
        _oldPosition = transform.position.x;
    }
 
    /// <summary>
    /// Detecta si la cámara se ha movido en el eje X.  
    /// Si ha cambiado, invoca el delegado `OnCameraTranslate` con la diferencia de posición.
    /// </summary>
    void Update()
    {
        if (transform.position.x != _oldPosition)
        {
            if (OnCameraTranslate != null)
            {
                float delta = _oldPosition - transform.position.x;
                OnCameraTranslate(delta);
            }
 
            _oldPosition = transform.position.x;
        }
    }

    #endregion
}