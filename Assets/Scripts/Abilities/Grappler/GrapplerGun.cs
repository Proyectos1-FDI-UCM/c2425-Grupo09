using UnityEngine;

public class Tutorial_GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Tutorial_GrapplingRope grappleRope;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;

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
    [SerializeField] private float impulso;      
    [SerializeField] private float offset;
    [SerializeField] private LayerMask grapplingLayer;   

    private Vector2 direction = Vector2.up;  

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    private void Update()
    {
        if (InputManager.Instance.GrapplerWasPressedThisFrame())
        {
            SetGrapplePoint();
        }
        else if (InputManager.Instance.GrapplerIsPressed())
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, true);
            }

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
        else if (InputManager.Instance.GrapplerWasReleasedThisFrame())
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

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

        Vector2 bestHitAdvanced = Vector2.zero;
        Vector2 bestHitPoint = bestHit.ClosestPoint(transform.position);

        for(float i = 1; i < 4; i++)
        {
            bestHitAdvanced = new Vector2 (bestHitPoint.x + offset/i * Mathf.Sign(m_rigidbody.gameObject.transform.rotation.y), bestHitPoint.y);

            Collider2D collider = Physics2D.OverlapCircle(bestHitAdvanced, 0.1f);
            float distance = Vector2.Distance(bestHitAdvanced, transform.position);

            if(collider != null && distance <= maxDistance)
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

        RotateGun(grapplePoint, true);
    }

    //Depuración puntos de enganche
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

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }

}
