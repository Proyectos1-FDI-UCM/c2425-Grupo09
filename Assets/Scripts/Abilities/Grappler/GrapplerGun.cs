//---------------------------------------------------------
// Este script es el responsable del grappler
// Sergio Valiente
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class GrapplerGun : MonoBehaviour
{

     // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("Scripts Ref:")]
    public GrapplerRope grappleRope;
    [SerializeField] private PlayerController playerController;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    [Header("Raycast")]
    [SerializeField] private float maxDistance;  
    [SerializeField] private float alturaMinimaEnganche;  

    [Header("Grappler")] 
    [SerializeField] private float impulso;      
    [SerializeField] private float offset;
    [SerializeField] private float velocidadReduccion;
    [SerializeField] private LayerMask grapplingLayer;   

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Vector2 _direction = Vector2.up; 

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (InputManager.Instance.GrapplerWasPressedThisFrame()) //Si se detecta el Input, se lanza el grappler
        {
            SetGrapplePoint();
        }
        else if (InputManager.Instance.GrapplerIsPressed()) //Mientras esta presionado el Input, se mantiene el grappler
        {
            if (launchToPoint && grappleRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
        else if (InputManager.Instance.GrapplerWasReleasedThisFrame()) //Si se deja de presionar el Input, se desactiva la cuerda
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
        }

        //Si el jugador esta enganchado, pero esta andando por el suelo
        if (grappleRope.isGrappling && playerController._enSuelo)
        {
            // Verifica si el jugador está moviéndose
            if (InputManager.Instance.MovementVector.x  != 0) 
                m_springJoint2D.frequency = 0.1f; //Hacemos flexible la cuerda para que pueda estirarse o contraerse
            
            //Si la cuerda se ha hecho demasiado grande, se suelta
            if(m_springJoint2D.distance > maxDistance) 
            {
                grappleRope.enabled = false;
                m_springJoint2D.enabled = false;
                m_rigidbody.gravityScale = 1;
            }

        } else m_springJoint2D.frequency = 0f; //Si no, se pone la frecuencia a 0 para que la cuerda deje de ser flexible

    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Este método establece el punto al que el jugador va a engancharse
    /// </summary>
    void SetGrapplePoint()
    {
        // Detectar todos los objetos en un radio determinado
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxDistance, grapplingLayer);
        
        if (hits.Length == 0) 
        {
            //Debug.Log("No se detectaron objetos en el área.");
            return;
        }

        Collider2D bestHit = null;
        float bestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            Vector2 hitPoint = hit.ClosestPoint(transform.position);

            // Filtrar solo los puntos que están por encima del jugador
            if (hitPoint.y < transform.position.y + alturaMinimaEnganche) continue;

            float distance = Vector2.Distance(hitPoint, transform.position);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestHit = hit;
            }
        }

        if (bestHit == null) 
        {
            //Debug.Log("No hay objetos válidos sobre el jugador.");
            return;
        }

        //Ahora buscamos un punto más avanzado del anterior (ya que sino como el jugador se mueve hacia delante, engancharse al punto anterior calculado resultaría en que el jugador se engancharía más atras)
        Vector2 bestHitAdvanced = Vector2.zero;
        Vector2 bestHitPoint = bestHit.ClosestPoint(transform.position);

        //Iteramos para ver varias posibilidades. Ej: el offset inicial es 3. Si este punto no es válido, entonces se prueba con 3/2, si este tampoco se prueba con 3/3 etc.
        for(float i = 1; i < 5; i++)
        {
            bestHitAdvanced = new Vector2 (bestHitPoint.x + offset/i * Mathf.Sign(m_rigidbody.gameObject.transform.rotation.y), bestHitPoint.y);

            //Ahora miramos que este nuevo punto este en un objeto al que engancharse
            Collider2D collider = Physics2D.OverlapCircle(bestHitAdvanced, 0.1f, grapplingLayer);
            float distance = Vector2.Distance(bestHitAdvanced, transform.position);

            if(collider != null && distance <= maxDistance) //Si se ha encontrado un punto válido, se establece y se sale del for
            {
                grapplePoint = bestHitAdvanced; 
                break;
            }
            else
            {
                grapplePoint = bestHitPoint;
            }
        }

        grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
        grappleRope.enabled = true;
    }

    /// <summary>
    /// Se dibuja con Gizmos para depurar el radio de accion, el punto más cercano al jugador, y el punto al que se une.
    /// </summary>
    void OnDrawGizmos()
    {
        // Dibujar el área de detección (círculo)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        // Dibujar todos los puntos detectados
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxDistance, grapplingLayer);
        foreach (Collider2D hit in hits)
        {
            Vector2 hitPoint = hit.ClosestPoint(transform.position);

            // Color amarillo para todos los puntos detectados
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(hitPoint, 0.1f);
        }

        // Dibujar el punto de enganche final en rojo
        if (grapplePoint != Vector2.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(grapplePoint, 0.15f);
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método que se encarga de realizar la acción de Grapple propiamente, es decir que lanza la cuerda para engancharse al punto deseado.
    /// </summary>
    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;

                //Impulso
                //m_rigidbody.velocity = new Vector2(impulso * Mathf.Sign(m_rigidbody.gameObject.transform.rotation.y), m_rigidbody.velocity.y);
            }

            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            if (launchType == LaunchType.Transform_Launch)
            {
                m_rigidbody.gravityScale = 0;
                m_rigidbody.velocity = Vector2.zero;
            }
            if (launchType == LaunchType.Physics_Launch)
            {
                m_springJoint2D.connectedAnchor = grapplePoint;
                m_springJoint2D.distance = 0;
                m_springJoint2D.frequency = launchSpeed;
                m_springJoint2D.enabled = true;
            }
        }
    }
    #endregion

}
