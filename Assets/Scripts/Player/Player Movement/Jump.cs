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
    [SerializeField] Transform _controlarSuelo;
    [SerializeField] LayerMask _suelo;
    [SerializeField] Vector3 _caja;
    [SerializeField] float _coyoteTime;
    [SerializeField] GameObject _prefab;

    // ---- ATRIBUTOS PRIVADOS ----
    private Rigidbody2D _rB;
    private bool _enSuelo;
    private float _coyoteCounter;
    private bool _canJump;
    


    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    private void Start()
    {
        _rB = GetComponent<Rigidbody2D>();
        
    }
    private void Update()
    {
        _enSuelo = Physics2D.OverlapBox(_controlarSuelo.position, _caja, 0f, _suelo);
        if (_enSuelo)
        { 
            _canJump = true;
            _coyoteCounter = _coyoteTime;
        }else
            _coyoteCounter -= Time.deltaTime;

        if (InputManager.Instance.JumpWasPressedThisFrame() && _canJump)
        {
            Instantiate(_prefab, gameObject.transform.position, gameObject.transform.rotation);
            if (_coyoteCounter > 0)
            {
                Salto();
            }
        }
        


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
        
            _rB.velocity = new Vector2(0, _alturaSalto);
        

        _canJump = false;   
    }

} // class Jump 
// namespace
