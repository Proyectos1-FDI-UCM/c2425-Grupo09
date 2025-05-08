//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Pablo Abellán, Diego García, Sergio Valiente
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System;
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
    [SerializeField]  float maxVelocidad = 7f;

    [SerializeField] Animator animator;
    public GameObject nightVision;

    public int ExtraJump
    {
        get => extraJump;
        set => extraJump = value;
    }

    public bool TigerUnlocked
    {
        get => tigerUnlocked;
        set => tigerUnlocked = value;
    }
    
    //Evento que se llama cuando cambia el estado de EnSuelo
    public event Action<bool> OnGroundStateChanged;

    public bool EnSuelo 
    {
        get => _enSuelo;
        set
        {
            if (_enSuelo != value) // Detecta cambio de estado
            {
                _enSuelo = value;
                OnGroundStateChanged?.Invoke(_enSuelo);

                if(_enSuelo == true)
                {
                    animator.SetTrigger("Land");
                    AudioManager.Instance.PlaySFX("land", true);
                } else animator.ResetTrigger("Land");
            }
        }
    }

    public bool FlippedRight
    {
        get => _flippedRight;
    }


    #endregion


    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Rigidbody2D _rB;
    private SpriteRenderer _sr;
    private GrapplerRope grappleRope;
    private float _coyoteCounter;
    private bool _isJumping;
    private bool _isFalling;
    private float _bufferCounter;

    private int _jumpCounter;

    private bool _enSuelo;
    private bool _flippedRight = true;
    private float moveX;
    private int extraJump = 0;
    private bool tigerUnlocked = false;
    
    private float tSpeed; 
    private float Speed;

    //Camara
    private CameraFollowObject _cameraFollowObject;
    private float _fallSpeedYDampingChangeThreshold;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _rB = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _cameraFollowObject = FindFirstObjectByType<CameraFollowObject>();
        grappleRope = GetComponentInChildren<GrapplerRope>();

        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Si no lo asignas en el inspector, lo asigna automáticamente
        }
        Speed = velocidad;
        tSpeed = velocidad * 1.25f;
        Abilities();
        _fallSpeedYDampingChangeThreshold = CameraManager.Instance.FallSpeedYDampingChangeThreshold;

    }
    private void Awake()
    {
        _rB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        EnSuelo = Physics2D.OverlapBox(ControlarSuelo.position, Caja, 0f, Suelo);
        //Debug.Log(EnSuelo);
        moveX = InputManager.Instance.MovementVector.x;

        //MOVIMIENTO CON GRAPPLER

        //Si no está enganchado o esta enganchado estando en el suelo se mueve normal
        if (!grappleRope.IsGrappling || (grappleRope.IsGrappling & EnSuelo))
        {   
            if(EnSuelo)
                _rB.velocity = new Vector2(velocidad * moveX, _rB.velocity.y); // Movimiento normal

            else if (Mathf.Abs(moveX) > 0) //Si está en el aire, solo pone velocidad al rb si el input es >0 (así conserva la inercia)
                _rB.velocity = new Vector2(velocidad * moveX, _rB.velocity.y); 

            else  //Si está en el aire y no hay input, lo deceleramos poco a poco
                _rB.velocity = new Vector2(Mathf.Lerp(_rB.velocity.x, 0, Time.deltaTime * 2f), _rB.velocity.y);
        }
        else //Si está enganchado, el jugador puede balancearse un poco añadiendo fuerzas (no velocidad para que se sienta mas natural)
        {
            if(Mathf.Abs(moveX) > 0)
                _rB.AddForce(new Vector2(moveX * velocidad/2f, 0), ForceMode2D.Force);
            else  
                _rB.velocity = new Vector2(Mathf.Lerp(_rB.velocity.x, 0, Time.deltaTime * 2f), _rB.velocity.y);
                //Si no recibe input estando colgado, se decelera poco a poco al jugador

            //Se limita la velocidad del jugador mientras este colgado para que no supere maxVelocidad
            if (_rB.velocity.magnitude > maxVelocidad)
            {
                _rB.velocity = _rB.velocity.normalized * maxVelocidad;
            }
        }
        
        if (moveX > 0 || moveX < 0) 
            TurnCheck();

        if (moveX != 0 && EnSuelo) animator.SetBool("Walk", true);

        else animator.SetBool("Walk", false);

    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
     void Update()
    {
        _coyoteCounter = EnSuelo ? CoyoteTime : _coyoteCounter - Time.deltaTime;

        _bufferCounter = InputManager.Instance.JumpWasPressedThisFrame() ? BufferTime : _bufferCounter - Time.deltaTime;

        if (_bufferCounter > 0 && _coyoteCounter > 0 && !_isJumping && !grappleRope.IsGrappling)
        {
            Jump();
            animator.SetTrigger("Jump");
            _bufferCounter = 0;
            StartCoroutine(JumpCooldown());
        }
        else if (InputManager.Instance.JumpWasPressedThisFrame() && !EnSuelo && !grappleRope.IsGrappling && _jumpCounter > 0)
        {
            Jump();
            animator.SetTrigger("DoubleJump");
            _jumpCounter = 0;
            StartCoroutine(JumpCooldown());
        }
        else if (EnSuelo)
        {
            _jumpCounter = extraJump;
        }

        if (tigerUnlocked) velocidad = tSpeed;
        else velocidad = Speed;

        if(_rB.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.Instance.IsLerpingYDamping && !CameraManager.Instance.LerpedFromPlayerFalling)
        {
            CameraManager.Instance.LerpYDamping(true);
        }

        if(_rB.velocity.y >= 0f && !CameraManager.Instance.IsLerpingYDamping && CameraManager.Instance.LerpedFromPlayerFalling)
        {
            CameraManager.Instance.LerpedFromPlayerFalling = false;
            CameraManager.Instance.LerpYDamping(false);
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public bool Tiger()
    { return tigerUnlocked; }

    public void DisablePlayer()
    {
        _rB.velocity = Vector2.zero;
        enabled = false;
    }

    public void UpdateJumpCounter()
    {
        _jumpCounter = extraJump;
    }

    public void Abilities()
    {

        if (AbilitiesManager.Instance.DoubleJump)
        {
            extraJump = 1;
        }
        if (AbilitiesManager.Instance.NightVision)
        {
            nightVision.SetActive(true);
        }
        if (AbilitiesManager.Instance.Tiger == true)
        {
            tigerUnlocked = true;
        }
    }

    /// <summary>
    /// Save se usa para guardar la posicion del jugador en el struct 
    /// </summary>
    public void Save(ref PlayerSaveData data)
    {
        data.Position = transform.position;
    }

    /// <summary>
    /// Load se usa para cargar la posicion del jugador en el struct 
    /// </summary>
    public void Load(PlayerSaveData data)
    {
        transform.position = data.Position;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Jump se llama cuando el jugador  quiere saltar.
    /// </summary>
    private void Jump()
    {
        AudioManager.Instance.PlaySFX("jump", true);
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

    private void TurnCheck()
    {
        if(moveX > 0 && !_flippedRight)
        {
            Turn();
        } else if(moveX < 0 && _flippedRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if(_flippedRight)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180f, transform.rotation.z));
            _flippedRight = !_flippedRight;
            _cameraFollowObject.CallTurn();
            //_sr.flipX = false;

        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0f, transform.rotation.z));
            _flippedRight = !_flippedRight;
            _cameraFollowObject.CallTurn();
            //_sr.flipX = true;
        }

       
    }
    #endregion   

}
// class NewBehaviourScript 
// namespace
[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 Position;
}