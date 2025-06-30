//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PhoenixAttackWave : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private GameObject fireWave;       // Prefab de la ola de fuego
    [SerializeField] private Transform player;          // Referencia al jugador
    [SerializeField] private float spawnDistance = 3f;  // Distancia a la izquierda o derecha del jugador
    [SerializeField] private float fireSpeed = 15f;      // Velocidad a la que viaja la ola
    public float spawnOffset;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float _timer = 0f;
    private float _spawnInterval = 2f;
    private Vector3 spawnPos;
    private SpriteRenderer _sR;

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
        _sR = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            _timer = 0f; // reinicia el temporizador
            SpawnFireWave();
            _timer = 0f; // reinicia el temporizador
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

    private void SpawnFireWave()
    {

        // Elegir aleatoriamente izquierda o derecha (0 = izquierda, 1 = derecha)
        int side = Random.Range(0, 2);
        spawnOffset = (side == 0) ? -1 : 1;

        if (player.position.y > 7.9f)
        {
            // Calcular posición de spawn al lado del jugador
            spawnPos = new Vector3(player.position.x + spawnOffset * spawnDistance, 7.83f, 0);
        }
        else if (player.position.y < 7.9f)
        {
            // Calcular posición de spawn al lado del jugador
            spawnPos = new Vector3(player.position.x + spawnOffset * spawnDistance, 1.816f, 0);
        }
        GameObject newWave = Instantiate(fireWave, spawnPos, Quaternion.identity);

        SpriteRenderer _sR = newWave.GetComponent<SpriteRenderer>();
        if (spawnOffset == -1)
        {
            _sR.flipX = true;
        }
        else if (spawnOffset == 1)
        {
            _sR.flipX = false;
        }

        // Calcular dirección hacia el jugador (en el plano X-Y)
        Vector3 direction = new Vector3((player.position.x - spawnPos.x), 0, 0).normalized;

        // Mover la ola hacia el jugador
        Rigidbody2D rb = newWave.GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireSpeed;
    }

    #endregion   

} // class PhoenixAttackWave 
// namespace
