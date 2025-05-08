//---------------------------------------------------------
// Este script se encarga de gestionar el sistema de Parallax. Lo lleva el objeto que contiene todos los layers del fondo.
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
 
[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private ParallaxCamera parallaxCamera;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
 
    /// <summary>
    /// Inicializa el sistema de parallax.  
    /// Obtiene la referencia de la cámara y se suscribe al evento de movimiento.  
    /// También configura las capas de parallax.
    /// </summary>
    void Start()
    {
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();
 
        if (parallaxCamera != null)
            parallaxCamera.OnCameraTranslate += Move;
 
        SetLayers();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
 
    /// <summary>
    /// Encuentra y almacena todas las capas de parallax como hijos del objeto padre. (Renombrandolos para mayor claridad)
    /// </summary>
    void SetLayers()
    {
        parallaxLayers.Clear();
 
        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();
 
            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }
 
    /// <summary>
    /// Mueve todas las capas de parallax en respuesta al desplazamiento de la cámara.
    /// </summary>
    void Move(float delta)
    {
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }

    #endregion

}