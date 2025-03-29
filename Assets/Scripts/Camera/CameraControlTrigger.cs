//---------------------------------------------------------
// Controlador de cámara basado en colisiones.
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using Cinemachine;
using UnityEditor;


/// <summary>
/// Controla el comportamiento de la cámara al detectar la entrada y salida de un jugador en un área específica.
/// Se encarga de cambiar de cámara o desplazar la vista en función de las configuraciones establecidas.
/// </summary>
public class CameraControlTrigger : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    public CustomInspectorObjects customInspectorObjects;

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
            if(customInspectorObjects.panCameraOnContact)
            {
                CameraManager.Instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
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
            if(customInspectorObjects.swapCameras && customInspectorObjects.cameraOnLeft != null && customInspectorObjects.cameraOnRight != null)
            {
                CameraManager.Instance.SwapCamera(customInspectorObjects.cameraOnLeft, customInspectorObjects.cameraOnRight, exitDirection);
            }

            if(customInspectorObjects.panCameraOnContact)
            {
                CameraManager.Instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
    
    #endregion   

} // class CameraControlTrigger 
// namespace

/// <summary>
/// Clase que almacena las configuraciones personalizadas del inspector para el control de la cámara.
/// Define opciones para intercambiar cámaras y desplazar la vista.
/// </summary>
[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

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

/// <summary>
/// Editor personalizado para la clase CameraControlTrigger.
/// Permite una mejor visualización y edición de las configuraciones de la cámara en el inspector de Unity.
/// </summary>
[CustomEditor(typeof(CameraControlTrigger))]
public class MyScriptEditor : Editor
{
    CameraControlTrigger cameraControlTrigger;

    private void OnEnable()
    {
        cameraControlTrigger = (CameraControlTrigger)target;
    }

    /// <summary>
    /// Dibuja la interfaz personalizada en el inspector de Unity, permitiendo la configuración de las cámaras y el desplazamiento.
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(cameraControlTrigger.customInspectorObjects.swapCameras)
        {
            cameraControlTrigger.customInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera on left", cameraControlTrigger.customInspectorObjects.cameraOnLeft,
            typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

            cameraControlTrigger.customInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera on right", cameraControlTrigger.customInspectorObjects.cameraOnRight, 
            typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }
        
        if(cameraControlTrigger.customInspectorObjects.panCameraOnContact)
        {
            cameraControlTrigger.customInspectorObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction",
            cameraControlTrigger.customInspectorObjects.panDirection);

            cameraControlTrigger.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraControlTrigger.customInspectorObjects.panDistance);
            cameraControlTrigger.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan Time", cameraControlTrigger.customInspectorObjects.panTime);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(cameraControlTrigger);
        }
    }
}
