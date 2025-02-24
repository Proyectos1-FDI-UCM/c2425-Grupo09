//---------------------------------------------------------
// Mecanica responsable de manejar la vida del jugador, es decir, de Noe.
// Alejandro García Díaz
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este script sera el responsable de manejar la vida del jugador,
/// aumentarla si se cura y reducirla si recibe daño
/// </summary>
public class Health : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----


    // ---- ATRIBUTOS PRIVADOS ----
    float maxHealth = 100f;

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //TEST -- Comprobar si aumenta la vida
        {
            maxHealth += 10f;
            Debug.Log("Vida restante: " +  maxHealth);
        }
        if (Input.GetKeyDown(KeyCode.D)) //TEST -- Comprobar si reduce la vida
        {
            maxHealth -= 10f;
            Debug.Log("Vida restante: " + maxHealth);
        }

        if(maxHealth <= 0) //Si la vida llega a 0 se destruye
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    // ---- MÉTODOS PÚBLICOS ----
    public void OnConsumable(int healthUp)
    {
        maxHealth += healthUp;
    }

    // ---- MÉTODOS PRIVADOS ----


} // class Health 
// namespace
