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
    #region Atributos del Inspector (serialized fields)

    [SerializeField] float maxHealth = 100f;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private float _fillSpeed;
    [SerializeField] private Gradient _colorGradient;    
    [SerializeField] private int appleHealthUp = 50;
    public int applesInInventory = 0;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float _currentHealth;

    #endregion
  
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start()
    {
        _currentHealth = maxHealth;
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
   
    public void Updatehealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, maxHealth);
        UpdateHealthBar();

        if (_currentHealth <= 0) //Si la vida llega a 0 se destruye
        {
            Debug.Log("Muerto");
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (InputManager.Instance.HealWasPressedThisFrame() && applesInInventory > 0) //Curacion al consumir una manzana
        {
            applesInInventory--;
            Updatehealth(appleHealthUp);
            FindObjectOfType<InventoryController>().AppleConsumed();
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos privados
    private void UpdateHealthBar()
    {
        float targetFillAmount = _currentHealth / maxHealth;
        _healthBarFill.DOFillAmount(targetFillAmount, _fillSpeed);
        _healthBarFill.color = _colorGradient.Evaluate(targetFillAmount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Apple"))
        {
            applesInInventory++;
            FindObjectOfType<InventoryController>().NumAppleToText(applesInInventory);
            Debug.Log("Manzana cogida");
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
    }
    #endregion

} // class Health 
// namespace
