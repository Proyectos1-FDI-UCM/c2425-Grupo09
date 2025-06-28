//---------------------------------------------------------
// Script responsable de controlar el tamaño y proporciones de la barra de vida al mejorarla o no
// Alejandro Garcia
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Runtime.CompilerServices;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class HealthBarController : MonoBehaviour
{
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float _posX = 483.3f;
    private float _width = 561.4f;
    private float _height = 78.6f;

    private float _posXUpgrade = 566.0283f;
    private float _widthUpgrade = 726.856f;

    private RectTransform _rt;
    private RectTransform _bgrt; // Background rt
    private RectTransform _shrt; // Shield rt

    [SerializeField] private GameObject normalHealth;
    [SerializeField] private GameObject upgradedHealth;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject shield;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Start()
    {
        _rt = GetComponent<RectTransform>();
        _bgrt = background.GetComponent<RectTransform>();
        _shrt = shield.GetComponent<RectTransform>();

        if (ConsumableManager.Instance.Grape)
        {
            UpgradeHealthBar();
        }
        else
        {
            NormalHealthBar();
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public void NormalHealthBar()
    {
        _rt.sizeDelta = new Vector2(_width, _height);
        _rt.position = new Vector2(237, gameObject.transform.position.y);
        _bgrt.sizeDelta = new Vector2(_width, _height);
        _bgrt.position = new Vector2(237, gameObject.transform.position.y);
        _shrt.sizeDelta = new Vector2(_width, _height);
        _shrt.position = new Vector2(237, gameObject.transform.position.y);
        Debug.Log("tu madre");
        upgradedHealth.SetActive(false);
        normalHealth.SetActive(true);
    }

    public void UpgradeHealthBar()
    {
        _rt.sizeDelta = new Vector2(_widthUpgrade, _height);
        _rt.position = new Vector2(267, gameObject.transform.position.y);
        _bgrt.sizeDelta = new Vector2(_widthUpgrade, _height);
        _bgrt.position = new Vector2(267, gameObject.transform.position.y);
        _shrt.sizeDelta = new Vector2(_widthUpgrade, _height);
        _shrt.position = new Vector2(267, gameObject.transform.position.y);
        upgradedHealth.SetActive(true);
        normalHealth.SetActive(false);
    }

    #endregion
} // class HealthBarController 
// namespace
