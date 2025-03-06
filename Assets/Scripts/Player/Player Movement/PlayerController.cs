//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Pablo Abellán y Diego García
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerController : MonoBehaviour
{


    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    //Velocidad del jugador al moverse
    [SerializeField] float velocidad;

    //Salto
    [SerializeField] float AlturaSalto;
    [SerializeField] Transform ControlarSuelo;
    [SerializeField] LayerMask Suelo;
    [SerializeField] Vector3 Caja;
    [SerializeField] float CoyoteTime;
    [SerializeField] float BufferTime;

    [SerializeField] Animator animator;

    #endregion


    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Rigidbody2D _rB;
    private bool _enSuelo;
    private float _coyoteCounter;
    private bool _isJumping;
    private float _bufferCounter;

    private int _extraJump = 1;
    private int _jumpCounter;
    
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _rB = GetComponent<Rigidbody2D>();
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Si no lo asignas en el inspector, lo asignamos automáticamente
        }
    }
    private void Awake()
    {
        _rB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _enSuelo = Physics2D.OverlapBox(ControlarSuelo.position, Caja, 0f, Suelo);

        float moveX = InputManager.Instance.MovementVector.x;

        _rB.velocity = new Vector2(velocidad * moveX, _rB.velocity.y);

        if (moveX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Walk", true); 
        }
        else if (moveX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool("Walk", true);
        }
        else animator.SetBool("Walk", false);

    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
     void Update()
    {
        _coyoteCounter = _enSuelo ? CoyoteTime : _coyoteCounter - Time.deltaTime;

        _bufferCounter = InputManager.Instance.JumpWasPressedThisFrame() ? BufferTime : _bufferCounter - Time.deltaTime;

        if (InputManager.Instance.JumpWasPressedThisFrame() && !_enSuelo && _jumpCounter > 0)
        {
            Jump();
            _jumpCounter = 0;
        }
        else if (_enSuelo)
        {
            _jumpCounter = _extraJump;
        }
        if (_bufferCounter > 0 && _coyoteCounter > 0 && !_isJumping)
        {
            Jump();
            _bufferCounter = 0;

            StartCoroutine(JumpCooldown());
        }

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
   /// <summary>
    /// Jump se llama cuando el jugador  quiere saltar.
    /// </summary>
    private void Jump()
    {
        _rB.velocity = new Vector2(_rB.velocity.x, AlturaSalto);
    }
    /// <summary>
    /// JumpCooldown se llama cuando el jugador ya ha saltado, 
    /// tenga que esperar un cooldown para volverlo a hacer.
    /// </summary>
    private IEnumerator JumpCooldown()
    {
        _isJumping = true;
        yield return new WaitForSeconds(0.4f);
        _isJumping = false;
    }
    #endregion   

} // class NewBehaviourScript 
// namespace
