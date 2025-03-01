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
    // Velocidad a la que se mueve el animal
    [SerializeField] float Speed;
    [SerializeField, Tooltip("Define el ancho del animal en unidades del juego desde el centro. Ej: Para un cubo 1x1x1 el ancho sería 0.5")]
    private float AnchoAnimal;
    [SerializeField, Tooltip("Define el alto del animal en unidades del juego desde el centro. Ej: Para un cubo 1x1x1 la altura sería 0.5")]
    private float AltoAnimal;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    //Vector para saber en qué direcion va el animal (izquierda o derecha)
    private Vector3 _direction = Vector3.right;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se mueve el animal a una velocidad constante, y en la direccion deseada. He gestionado el movimiento por transform en vez de por físicas
    /// al tratarse de un patrón de movimiento sencillo
    /// </summary>
    void Update()
    {
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
        
        // Dibujar el rayo para depuración
        //Debug.DrawRay(origen, direccion * distancia, Color.red);

        return hit.collider != null; 
    }

    #endregion   

} // class AnimalController 
// namespace
