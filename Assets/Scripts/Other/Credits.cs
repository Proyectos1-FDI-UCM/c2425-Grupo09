//---------------------------------------------------------
// Script que llevan los créditos para poder llamar métodos en los eventos del animador
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Credits : MonoBehaviour
{

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    
    public void EndCredits()
    {
        SceneManager.LoadScene("MainMenu");
        InputManager.Instance.EnableUIControls();
    }

    #endregion

} // class Credits 
// namespace
