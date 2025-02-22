//---------------------------------------------------------
// Proporciona la mecánica de salto al jugador con un parámetro modificable desde el inspector(altura del salto)
// Diego García Alonso
// The Last Vessel

// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.InputSystem;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Jump : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    [SerializeField] float _alturaSalto;
    [SerializeField] Rigidbody2D _rB;
    [SerializeField] Transform _controlarSuelo;
    [SerializeField] LayerMask _suelo;
    // ---- ATRIBUTOS PRIVADOS ----
    private InputActionSettings _inputAction;
    private bool _enSuelo;
    
    private Vector3 _caja;
    
    

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    private void Awake()
    {
        _inputAction = new InputActionSettings();

        _inputAction.Player.Jump.performed += ctx =>
        {
            Salto();
        };
    }
    private void Start()
    {
        _rB = GetComponent<Rigidbody2D>();
        
    }
    private void Update()
    {
        _enSuelo = Physics2D.OverlapBox(_controlarSuelo.position, _caja,0f,_suelo);
    }

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    private void Salto()
    {
        if (_enSuelo)
        {
            _rB.AddForce(new Vector2(0, _alturaSalto), ForceMode2D.Impulse);
        }

    }
    private void OnEnable() => _inputAction.Player.Enable();
    private void OnDisable() => _inputAction.Player.Disable();

} // class Jump 
// namespace
