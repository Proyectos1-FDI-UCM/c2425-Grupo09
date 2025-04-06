//---------------------------------------------------------
// Script que se encarga de generar un temblor en la cámara.
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    public static CameraShakeManager Instance;

    #endregion

    #region TESTING

    //Esta region es solo para testear que funciona, luego se borrará
    [SerializeField] private float _shakeForce = 1f;
    private CinemachineImpulseSource _cinemachineImpulseSource;
    
    void Start()
    {
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if(InputManager.Instance.TestingWasPressedThisFrame())
        CameraShake(_cinemachineImpulseSource, _shakeForce);
    }

    #endregion
    
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Patrón Singleton.
    /// </summary>
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Genera un temblor en la cámara.
    /// </summary>
    /// <param name="impulseSource"> El componente que genera el temblor. </param>
    /// <param name="force">fuerza del temblor(por defecto 1f)</param>
    public void CameraShake(CinemachineImpulseSource impulseSource, float force = 1f)
    {
        impulseSource.GenerateImpulseWithForce(force);
    }

    public void StandardCameraShake()
    {
        _cinemachineImpulseSource.GenerateImpulseWithForce(_shakeForce);
    }

    #endregion

} // class CameraShakeManager 
// namespace
