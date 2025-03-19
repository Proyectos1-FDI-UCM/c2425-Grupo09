//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// Clase encargada de la interacción con un objeto animal en el juego. 
/// Permite recoger el objeto cuando el jugador se acerca y presiona la tecla 'F'.
/// </summary>
public class Capture : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private float distanciaMaxima = 30f;  // Distancia máxima a la que se puede interactuar con el objeto.
    [SerializeField] Animator animator;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private bool _near = false;  
    private GameObject _animal;  
    private BarraDeSueño _barraDeSueño; 
    private PlayerController _playerController;
    private GrapplerGun _grapplerGun;
    private Health _health;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _grapplerGun = GetComponentInChildren<GrapplerGun>();
        _health = GetComponent<Health>();
    }

    /// <summary>
    /// Update se llama cada frame si el MonoBehaviour está habilitado.
    /// </summary>
    void Update()
    {
        if (_animal != null)
        {
            float distancia = Vector3.Distance(transform.position, _animal.transform.position);

            if (distancia <= distanciaMaxima)
            {
                _near = true;
            }
            else
            {
                _near = false;
            }

            if (_near && InputManager.Instance.CaptureWasPressedThisFrame () && _barraDeSueño.Dormido())
            {
                animator.SetTrigger("Capture");
                CheckpointManager.Instance.SetCheckpoint(transform.position);
                
                if (_animal.CompareTag("Bunny"))
                {
                    Debug.Log("saltos extra:" + _playerController.extraJump);
                    AbilitiesManager.Instance.BunnyAbilityUnlock();
                    _playerController.extraJump = 1;
                    Debug.Log("saltos extra:" + _playerController.extraJump);
                }
                if (_animal.CompareTag("Bat"))
                {
                    Debug.Log("activada la vision nocturna");
                    AbilitiesManager.Instance.BatAbilityUnlock();
                    _playerController.nightVision.SetActive(true);
                    Debug.Log("activada la vision nocturna");
                }
                if (_animal.CompareTag("Gorila"))
                {
                    AbilitiesManager.Instance.GorilaAbilityUnlock();
                    _grapplerGun.grapplerUnlocked = true;
                }
                if (_animal.CompareTag("Tiger"))
                {
                    AbilitiesManager.Instance.TigerAbilityUnlock();
                    _playerController.tigerUnlocked = true;
                }
                if (_animal.CompareTag("Armadillo"))
                {
                    AbilitiesManager.Instance.ArmadilloAbilityUnlock();
                    _health.armadilloUnlocked = true;
                }
                RecogerObjeto();
            }
        }
        //Debug.Log(_animal);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<BarraDeSueño>() != null)
        {
            _animal = other.gameObject; 
            _barraDeSueño = _animal.GetComponent<BarraDeSueño>();
        }
    }

    /// <summary>
    /// Método llamado cuando el jugador sale de la zona de colisión.
    /// </summary>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<BarraDeSueño>() != null)
        {
            _near = false;  
            _animal = null;  
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// </summary>
    void RecogerObjeto()
    {
        if (_animal != null)
        {
            Destroy(_animal);
        }
    }

    #endregion

} // class Capture

