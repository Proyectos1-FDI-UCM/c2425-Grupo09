//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PhoenixController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private float phoenixSpeed;
    [SerializeField] private BoxCollider2D arenaArea;
    [SerializeField] private GameObject health;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    // Las tres posiciones posibles (horizontalmente separadas 15 unidades)
    private Vector3[] _positions = new Vector3[3];

    // Duración de espera en cada posición
    private float _waitTime = 6f;

    // Velocidad de movimiento
    private float _moveSpeed = 4f;

    // Posición actual (índice)
    private int _currentIndex;

    // Controlador de animaciones
    private Animator _animator;

    private bool _playerDetected = false;
    private bool _dead = false;

    private SpriteRenderer _sR;
    private BarraDeSueño _sueño;

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
        // Establecemos las posiciones
        Vector3 basePos = transform.position;
        _positions[0] = new Vector3(30, basePos.y, basePos.z);
        _positions[1] = new Vector3(45, basePos.y, basePos.z);
        _positions[2] = new Vector3(60, basePos.y, basePos.z);

        _animator = GetComponent<Animator>();
        _sR = GetComponent<SpriteRenderer>();
        _sueño = GetComponent<BarraDeSueño>();

        // Detectar la posición inicial más cercana
        _currentIndex = GetNearestPositionIndex(transform.position);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Debug.Log(_sueño.barraDeSueño);

        if (_sueño.barraDeSueño >= 300 && !_dead)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position, _moveSpeed * Time.deltaTime);
            StopCoroutine(MoveLoop());
            health.SetActive(false);
            _dead = true;
            _animator.SetTrigger("Death");
            Destroy(gameObject, 1.5f);
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private IEnumerator MoveLoop()
    {
        Debug.Log("empiza bucle");
        while (true)
        {
            // Elegir una nueva posición distinta de la actual
            int nextIndex;
            do
            {
                nextIndex = Random.Range(0, _positions.Length);
            } while (nextIndex == _currentIndex);

            // Mover hacia esa posición
            yield return StartCoroutine(MoveToPosition(_positions[nextIndex]));

            // Actualizar posición actual
            _currentIndex = nextIndex;

            // Empieza la animacion de idle
            _animator.SetBool("Flight", false);

            // Esperar antes de volver a moverse
            yield return new WaitForSeconds(_waitTime);
        }
    }
    private IEnumerator MoveToPosition(Vector3 target)
    {
        _animator.SetBool("Flight", true);
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            if (target.x - transform.position.x < 0) { _sR.flipX = true; }
            if (target.x - transform.position.x > 0) { _sR.flipX = false; }
            transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Corregir posición final
        transform.position = target;
    }

    private int GetNearestPositionIndex(Vector3 pos)
    {
        int closest = 0;
        float minDist = Vector3.Distance(pos, _positions[0]);
        for (int i = 1; i < _positions.Length; i++)
        {
            float dist = Vector3.Distance(pos, _positions[i]);
            if (dist < minDist)
            {
                closest = i;
                minDist = dist;
            }
        }
        return closest;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("choque");
        if (!_playerDetected && other.GetComponent<PlayerController>() != null)
        {
            _playerDetected = true;
            arenaArea.enabled = false;
            StartCoroutine(MoveLoop());
        }
    }

    private void PhoenixDead()
    {
        health.SetActive(false);
        StartCoroutine(Death());
        Destroy(gameObject);
    }

    private IEnumerator Death()
    {
        _animator.SetTrigger("Death");
        yield return new WaitForSecondsRealtime(0.45f);
    }

} // class PhoenixController 
// namespace
