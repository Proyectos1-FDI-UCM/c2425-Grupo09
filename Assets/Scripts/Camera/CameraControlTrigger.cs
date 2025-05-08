//---------------------------------------------------------
// Controlador de cámara basado en colisiones.
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using Cinemachine;

/// <summary>
/// Controla el comportamiento de la cámara al detectar la entrada y salida de un jugador en un área específica.
/// Se encarga de cambiar de cámara o desplazar la vista en función de las configuraciones establecidas.
/// </summary>
public class CameraControlTrigger : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("Camera Swap")]
    [SerializeField] private bool swapCameras = false;
    [SerializeField] private CinemachineVirtualCamera cameraOnLeft;
    [SerializeField] private CinemachineVirtualCamera cameraOnRight;

    [Header("Camera Pan")]
    [SerializeField] private bool panCameraOnContact = false;

    [SerializeField] private PanDirection panDirection;
    [SerializeField] private float panDistance = 3f;
    [SerializeField] private float panTime = 0.35f;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Collider2D _coll;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    private void Start()
    {
        _coll = GetComponent<Collider2D>();
    }
    
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Detecta cuándo un objeto con un PlayerController entra en la zona de activación.
    /// Si la opción de mover la cámara está habilitada, ajusta la vista en la dirección configurada.
    /// </summary>
    /// <param name="other">Collider del objeto que entra en la zona.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            if(panCameraOnContact)
            {
                CameraManager.Instance.PanCameraOnContact(panDistance, panTime, panDirection, false);
            }
        }
    }

    /// <summary>
    /// Detecta cuándo un objeto con un PlayerController sale de la zona de activación.
    /// Si la opción de cambiar de cámara está habilitada, intercambia entre las cámaras configuradas.
    /// También permite volver a mover la cámara si está activado.
    /// </summary>
    /// <param name="other">Collider del objeto que sale de la zona.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            Vector2 exitDirection = (other.transform.position - _coll.bounds.center).normalized;
            if(swapCameras && cameraOnLeft != null && cameraOnRight != null)
            {
                CameraManager.Instance.SwapCamera(cameraOnLeft, cameraOnRight, exitDirection);
            }

            if(panCameraOnContact)
            {
                CameraManager.Instance.PanCameraOnContact(panDistance, panTime, panDirection, true);
            }
        }
    }
    
    #endregion   

} // class CameraControlTrigger 
// namespace

/// <summary>
/// Enumeración que define las posibles direcciones en las que se puede mover la cámara.
/// </summary>
public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}

