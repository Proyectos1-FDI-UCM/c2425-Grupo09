//---------------------------------------------------------
// Este script se encarga de gestionar el sistema de Parallax. Lo lleva cada layer del fondo.
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
public class ParallaxLayer : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private float parallaxFactor;

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
 
    /// <summary>
    /// Mueve el objeto en el eje X aplicando un efecto de parallax.
    /// </summary>
    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor;
 
        transform.localPosition = newPos;
    }

    #endregion

}