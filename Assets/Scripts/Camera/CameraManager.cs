//---------------------------------------------------------
// Gestiona las camaras del juego
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
using Cinemachine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Gestiona las cámaras virtuales en la escena, permitiendo cambios dinámicos entre ellas
/// y ajustes en el comportamiento de la cámara en función del movimiento del jugador.
/// </summary>
public class CameraManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    public static CameraManager Instance;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera[] VirtualCameras;

    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float FallSpeedYDampingChangeThreshold = -10f;

    public bool IsLerpingYDamping { get; private set; } 
    public bool LerpedFromPlayerFalling { get; set; }

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Coroutine _lerpYPanCoroutine;
    private Coroutine _panCameraCoroutine;
    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;
    private float _normYPanAmount;

    private Vector2 _startingTrackedObjectOffset;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Configura la instancia del CameraManager y asigna la cámara activa al inicio.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        for(int i = 0; i < VirtualCameras.Length; i++)
        {
            if(VirtualCameras[i].enabled)
            {
                _currentCamera = VirtualCameras[i];
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        _normYPanAmount = _framingTransposer.m_YDamping;

        _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    
    /// <summary>
    /// Modifica suavemente el Y Damping de la cámara dependiendo de si el jugador está cayendo.
    /// </summary>
    /// <param name="isPlayerFalling">Indica si el jugador está cayendo.</param>
    
    //Lerp Camera
    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    } 

     /// <summary>
    /// Realiza un desplazamiento de la cámara en una dirección específica durante un tiempo determinado.
    /// </summary>
    /// <param name="panDistace">Distancia del desplazamiento.</param>
    /// <param name="panTime">Tiempo que tarda en completarse el desplazamiento.</param>
    /// <param name="panDirection">Dirección del desplazamiento.</param>
    /// <param name="panToStartingPos">Si es verdadero, la cámara vuelve a su posición original.</param>
    public void PanCameraOnContact(float panDistace, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistace, panTime, panDirection, panToStartingPos));
    }

    /// <summary>
    /// Cambia entre dos cámaras virtuales en función de la dirección de salida del jugador.
    /// </summary>
    /// <param name="cameraFromLeft">Cámara activa cuando el jugador viene desde la izquierda.</param>
    /// <param name="cameraFromRight">Cámara activa cuando el jugador viene desde la derecha.</param>
    /// <param name="triggerExitDirection">Dirección en la que el jugador sale del trigger.</param>
    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        if(_currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            cameraFromRight.enabled = true;
            cameraFromLeft.enabled = false;
            _currentCamera = cameraFromRight;
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        } 
        else if(_currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            cameraFromLeft.enabled = true;
            cameraFromRight.enabled = false;
            _currentCamera = cameraFromLeft;
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Corrutina que ajusta el valor de Y Damping de la cámara suavemente.
    /// </summary>
    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }

        float elapsedTime = 0f;
        while(elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));
            _framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }
        IsLerpingYDamping = false;
    }

    /// <summary>
    /// Corrutina que desplaza la cámara en la dirección y distancia especificadas.
    /// </summary>
    private IEnumerator PanCamera(float panDistace, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if(!panToStartingPos)
        {
            switch(panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.right;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.left;
                    break;
            }

            endPos *= panDistace;
            startingPos = _startingTrackedObjectOffset;
            endPos += startingPos;  
        }
        else
        {
            startingPos = _startingTrackedObjectOffset;
            endPos = _startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLep = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            _framingTransposer.m_TrackedObjectOffset = panLep;

            yield return null;
        }
    }
    #endregion   

} // class CameraManager 
// namespace
