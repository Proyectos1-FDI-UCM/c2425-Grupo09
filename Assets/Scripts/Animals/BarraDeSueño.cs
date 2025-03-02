//---------------------------------------------------------
// Se encarga de gestionar la barra de sueño de los animales
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
public class BarraDeSueño : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] int MaxBarraDeSueño;
    [SerializeField] Collider2D TriggerCollider;
    
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    
    [SerializeField] private int _barraDeSueño = 0; 
    //De momento está SerializeField para poder comprobar en el inspector que aumenta correctamente. Luego se quitará.
    private AnimalController _animalController;
    private bool dormido;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    void Awake()
    {
        _animalController = GetComponent<AnimalController>();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método que se llama desde el script Bullet para aumentar el sueño del animal.
    /// </summary>
    public void Dormir(int amount)
    {
        if(_barraDeSueño < MaxBarraDeSueño)
        _barraDeSueño += amount;

        else if(!dormido)
        {
            _animalController.enabled = false;
            TriggerCollider.enabled = false;
            dormido = true;
        }
          
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados


    #endregion   

} // class BarraDeSueño 
// namespace
