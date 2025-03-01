//---------------------------------------------------------
// Mecanica responsable de manejar la vida del jugador, es decir, de Noe.
// Alejandro García Díaz, y Sergio González
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este script sera el responsable de manejar la vida del jugador,
/// aumentarla si se cura y reducirla si recibe daño
/// </summary>
public class Health : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    [SerializeField] float maxHealth = 100f;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private float _fillSpeed;
    [SerializeField] private Gradient _colorGradient;
    // ---- ATRIBUTOS PRIVADOS ----
    private float _currentHealth;
  
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    void Start()
    {
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        if (InputManager.Instance.FireWasPressedThisFrame()) //TEST -- Comprobar si aumenta la vida
        {
            Updatehealth(10);
            Debug.Log("Vida restante: " +  _currentHealth);
        }
        if (InputManager.Instance.JumpWasPressedThisFrame()) //TEST -- Comprobar si reduce la vida
        {
            Updatehealth(-10);
            Debug.Log("Vida restante: " + _currentHealth);
        }
        if (_currentHealth <= 0) //Si la vida llega a 0 se destruye
        {
            Debug.Log("Muerto");
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }

    // ---- MÉTODOS PÚBLICOS ----
    public void OnConsumable(int healthUp)
    {
        _currentHealth += healthUp;
    }

    // ---- MÉTODOS PRIVADOS ----
    private void Updatehealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float targetFillAmount = _currentHealth / maxHealth;
        _healthBarFill.DOFillAmount(targetFillAmount, _fillSpeed);
        _healthBarFill.color = _colorGradient.Evaluate(targetFillAmount);
    }

} // class Health 
// namespace
