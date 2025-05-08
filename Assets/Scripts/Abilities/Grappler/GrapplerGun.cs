//---------------------------------------------------------
// Este script es el responsable del grappler
// Sergio Valiente
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Esta clase gestiona el grappler del jugador.
/// </summary>
public class GrapplerGun : MonoBehaviour
{

     // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("Scripts Ref:")]
    [SerializeField] private GrapplerRope grappleRope;
    [SerializeField] private PlayerController playerController;

    [Header("Main Camera:")]
    private Camera m_camera;
    public Camera MainCamera
    {
        get => m_camera;
    }

    [Header("Transform Ref:")]
    [SerializeField] private Transform gunHolder;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform firePoint;

    public Transform GunHolder
    {
        get => gunHolder;
    }
    public Transform GunPivot
    {
        get => gunPivot;
    }
    public Transform FirePoint
    {
        get => firePoint;
    }

    [Header("Physics Ref:")]
    [SerializeField] private SpringJoint2D m_springJoint2D;
    [SerializeField] private Rigidbody2D m_rigidbody;

    [SerializeField] private bool grapplerUnlocked = false;

    public SpringJoint2D M_springJoint2D
    {
        get => m_springJoint2D;
    } 

    public Rigidbody2D M_rigidbody
    {
        get => m_rigidbody;
    }

    public bool GrapplerUnlocked
    {
        get => grapplerUnlocked;
        set => grapplerUnlocked = value;
    }

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }
    //Esto es si quisieramos que el grappler impulsara al jugador hacia la dirección donde se ha enganchado
    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] private Vector2 grapplePoint;
    [HideInInspector] private Vector2 grappleDistanceVector;

    public Vector2 GrapplePoint
    {
        get => grapplePoint;
    }
    public Vector2 GrappleDistanceVector
    {
        get => grappleDistanceVector;
    }

    [Header("Raycast")]
    [SerializeField] private float maxDistance;  
    [SerializeField] private float alturaMinimaEnganche;  

    [Header("Grappler")] 
    [SerializeField] private float impulso;      
    [SerializeField] private float offset;
    [SerializeField] private float flexibilidadAndando = 0.3f;
    [SerializeField] private LayerMask grapplingLayer;   

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Vector2 _direction = Vector2.up; 
    private float _initialGravity;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Al pincipio se desactiva la cuerda y se desactiva el springJoint2D. Además se suscribe a los eventos de OnGroundStateChanged y OnGrappleStateChanged.
    /// </summary>
    private void Start()
    {
        //Al principio se desactiva la cuerda
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;

        //Suscripción a eventos
        playerController.OnGroundStateChanged += CheckGrapplingState;
        grappleRope.OnGrappleStateChanged += CheckGrapplingState;

        //Comprobacion de desbloqueo de la habilidad
        Abilities();

        _initialGravity = m_rigidbody.gravityScale;
    }

    /// <summary>
    /// En el update se actualiza el estado del grappler en función del input del jugador.
    /// </summary>
    private void Update()
    {
        //Si se detecta el Input, se lanza el grappler

        if (InputManager.Instance.GrapplerWasPressedThisFrame() && grapplerUnlocked) 
        {
            SetGrapplePoint();
        }
        //Mientras esta presionado el Input, se mantiene el grappler
        else if (InputManager.Instance.GrapplerIsPressed() && grapplerUnlocked) 
        {
            if (launchToPoint && grappleRope.IsGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
        //Si se deja de presionar el Input, se desactiva la cuerda
        else if (InputManager.Instance.GrapplerWasReleasedThisFrame() && grapplerUnlocked) 
        {
            HUDAbilities.Instance.GorillaGlow(false);
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = _initialGravity;
        }

        //Si la cuerda se ha hecho demasiado grande, se suelta. 
        //De momento esta desactivado porque la flexibilidad de la cuerda a 0.3 ya limita el tamaño, pero si la hicieramos mas flexible si sería necesario.

        /*if(m_springJoint2D.distance > maxDistance * 1.5f) 
        {
            HUDAbilities.Instance.GorillaGlow(false);
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = _initialGravity;
        }*/
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Método que se llama en los eventos playerController.OnGroundStateChanged y grappleRope.OnGrappleStateChanged.
    /// Si el jugador está enganchado y en el suelo se hace la cuerda flexible, sino rígida. 
    /// <summary>
    private void CheckGrapplingState(bool _)
    {        
        if (grappleRope.IsGrappling && playerController.EnSuelo)
        {
            m_springJoint2D.frequency = flexibilidadAndando; //Se hace la cuerda flexible
        } 
        else 
        {
            m_springJoint2D.distance = Vector2.Distance(grapplePoint, transform.position); //Se reestablece el tamaño de la cuerda cuando el jugador deja de estar en el suelo
            m_springJoint2D.frequency = 0f; //Si no, se pone la frecuencia a 0 para que la cuerda deje de ser flexible
        }
    }

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
        
        HUDAbilities.Instance.GorillaGlow(true);
        AudioManager.Instance.PlaySFX("launchGrappler", true);
        playerController.UpdateJumpCounter();
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
        else //Esto es si quisieramos que el grappler impulsara al jugador hacia la dirección donde se ha enganchado
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

    /// <summary>
    /// Método para desactivar el grappler. Se llama desde health cuando el jugador muere.
    /// </summary>
    public void ReleaseGrapple()
    {
        HUDAbilities.Instance.GorillaGlow(false);
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        m_rigidbody.gravityScale = _initialGravity;
    }
    
    public void Abilities()
    {
        if (AbilitiesManager.Instance.Grappler == true)
        {
            grapplerUnlocked = true;
        }
    }
    #endregion

}
