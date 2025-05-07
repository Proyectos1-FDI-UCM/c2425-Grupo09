//---------------------------------------------------------
// Responsable de manejar todo el contenido que muestra el inventario en pantalla (numero de manzanas, etc...)
// Alejandro García Díaz
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class InventoryController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] TextMeshProUGUI numApples;
    [SerializeField] GameObject grayGoldenApple;
    [SerializeField] private int appleHealthUp = 50;
    [SerializeField] GameObject[] itemArray;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Health _health;
    private Animator _animator;
    
    private int aviableApples;
    private int applesInInventory = 0;
    private List<int> _itemList = new List<int>();
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Awake()
    {
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (InputManager.Instance.HealWasPressedThisFrame() && applesInInventory > 0 && _health._currentHealth < 100) //Curacion al consumir una manzana
        {
            _animator.SetTrigger("Eat");
            AudioManager.Instance.PlaySFX("eat", true);
            applesInInventory--;
            _health.Updatehealth(appleHealthUp);
            AppleConsumed();
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public void AddAppleToInventory()
    {
        applesInInventory++;
        NumAppleToText(applesInInventory);
    }

    public void InventoryAdd(int id) 
    { 
        _itemList.Add(id);
    }
    #region Save and Load

    public void Save(ref ItemData data)
    {
        data.apple = applesInInventory;
        data.items = _itemList;
    }
    public void Load(ItemData data)
    {
        applesInInventory = data.apple;
        _itemList = data.items;
        NumAppleToText(applesInInventory);
        for (int i = 0; i < _itemList.Count; i++)
        {
            Destroy(itemArray[_itemList[i]]);
        }
    }


    #endregion

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private void AppleConsumed()
    {
        aviableApples--;
        NumAppleToText(aviableApples);
    }

    private void NumAppleToText(int applesInInventory)
    {
        if (applesInInventory > 0)
        {
            aviableApples = applesInInventory;
            grayGoldenApple.SetActive(false);
            numApples.gameObject.SetActive(true);
            numApples.text = Convert.ToString(aviableApples);
        }
        else
        {
            numApples.gameObject.SetActive(false);
            grayGoldenApple.SetActive(true);
        }
    }

    #endregion   

} // class InventoryController 
// namespace
[System.Serializable]
public struct ItemData
{
    public List<int> items;
    public int apple;
}
