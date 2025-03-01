//---------------------------------------------------------
// Este script se encarga de gestionar el movimiento y ataque de los animales
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class AnimalController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [Header("Movimiento")]
    // Velocidad a la que se mueve el animal
    [SerializeField] float Speed;
    [SerializeField, Tooltip("Define la mitad del ancho del animal en unidades del juego. Ej: Para un cubo 1x1x1 el ancho sería 0.5")]
    private float AnchoAnimal;
    [SerializeField, Tooltip("Define la mitad del alto del animal en unidades del juego. Ej: Para un cubo 1x1x1 la altura sería 0.5")]
    private float AltoAnimal;
    [Header("Ataque")]
    [SerializeField] float DistanciaDeteccion;
    [SerializeField] float CooldownAtaque;
    [SerializeField] int Daño;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    //Vector para saber en qué direcion va el animal (izquierda o derecha)
    private Vector3 _direction = Vector3.right;
    private float _tiempoUltimoAtaque;
    private bool _isInAttackRange = false;
    private Transform _player;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Awake()
    {
        _player = FindFirstObjectByType<Health>().gameObject.transform;
    }

    /// <summary>
    /// Se mueve el animal a una velocidad constante, y en la direccion deseada. He gestionado el movimiento por transform en vez de por físicas
    /// al tratarse de un patrón de movimiento sencillo
    /// </summary>
    void Update()
    {
        if(DetectarJugador(Vector2.right * Mathf.Sign(_direction.x), DistanciaDeteccion, "Player", "Ground"))
        { 
            if(!_isInAttackRange)
           transform.position = Vector2.MoveTowards(transform.position, _player.position, Speed * Time.deltaTime);
        } else 
            transform.position += _direction * Speed * Time.deltaTime;
        
        // Si(Detecta muro || Deja de detectar plataforma)
        if (DetectarObjeto(Vector2.right * Mathf.Sign(_direction.x), Vector3.zero, AnchoAnimal, "Ground") || !DetectarObjeto(Vector2.down, new Vector3(AnchoAnimal + 0.2f, 0, 0) * Mathf.Sign(_direction.x), AltoAnimal + 0.2f, "Ground"))
        {
            // Gira el enemigo y cambiar la dirección
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
            _direction *= -1;
        }
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

     /// <summary>
    /// Este método se encarga de tirar un Raycast en la direccion Vector2 direccion, desplazado un Vector3 offset desde el transform
    /// que llega a una distancia float distancia, y que detecta los objetos del layer string nombreCapa.
    /// </summary>
    private bool DetectarObjeto(Vector2 direccion, Vector3 offset, float distancia, string nombreCapa)
    {
        Vector2 origen = transform.position + offset; 
        LayerMask capa = LayerMask.GetMask(nombreCapa);
        
        RaycastHit2D hit = Physics2D.Raycast(origen, direccion, distancia, capa);
        
        //Dibujar el rayo para depuración
        Debug.DrawRay(origen, direccion * distancia, Color.yellow);

        return hit.collider != null; 
    }

    /// <summary>
    /// Este método se encarga de tirar un Raycast en la direccion Vector2 direccion,
    /// que llega a una distancia float distancia, y que detecta los objetos del layer string capaJugador y capaPared.
    /// Solo devuelve true si el jugador es el primero que detecta (si está detrás de una pared no lo detectaria)
    /// </summary>
    private bool DetectarJugador(Vector2 direccion, float distancia, string capaJugador, string capaPared)
    {
        Vector2 origen = transform.position; 
        LayerMask capasDetectables = LayerMask.GetMask(capaJugador, capaPared);
        
        RaycastHit2D hit = Physics2D.Raycast(origen, direccion, distancia, capasDetectables);
        //Dibujar el rayo para depuración
        Debug.DrawRay(origen, direccion * distancia, Color.green);
        
        if (hit.collider != null)
        {
            // Si el primer objeto detectado es el jugador, retorna true
            if (hit.collider.gameObject.GetComponent<Health>() != null) 
            {
                return true;
            }
        }

        return false; // No detectó al jugador o fue bloqueado por una pared
    }

    /// <summary>
    /// OnTriggerEnter2D se llama cuando algun objeto entra en el collider en modo IsTrigger del animal (que representa el rango de ataque).
    /// </summary>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.GetComponent<Health>() != null)
            _isInAttackRange = true;
    }

    /// <summary>
    /// Si el jugador se mantiene dentro del Collider, ataca a cooldown y actualiza la vida del jugador con el daño que le hace
    /// </summary>
    private void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.gameObject.GetComponent<Health>() != null && Time.time > _tiempoUltimoAtaque + CooldownAtaque)
        {
            coll.gameObject.GetComponent<Health>().Updatehealth(-Daño);
            _tiempoUltimoAtaque = Time.time;
        }
    }

    /// <summary>
    /// Si el jugador sale del collider IsTrigger, el bool se pone en false para que el animal vuelva a moverse
    /// </summary>
    private void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.gameObject.GetComponent<Health>())
        {
            _isInAttackRange = false;
        }
    }


    #endregion   

} // class AnimalController 
// namespace
