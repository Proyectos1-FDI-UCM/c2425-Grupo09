//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín , DIego García
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Componente que se encarga de la gestión de un nivel concreto.
/// Este componente es un singleton, para que sea accesible para todos
/// los objetos de la escena, pero no tiene el comportamiento de
/// DontDestroyOnLoad, ya que solo vive en una escena.
///
/// Contiene toda la información propia de la escena y puede comunicarse
/// con el GameManager para transferir información importante para
/// la gestión global del juego (información que ha de pasar entre
/// escenas)
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;
    private PlayerController _playerController;
    private AbilitiesManager _abilitiesManager;
    private Timer _timer;
    private Health _health;
    private Capture _capture;
    private GrapplerGun _gun;
    private InventoryController _inventoryController;
    private CheckList _checkList;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
            Init();
        }
    }
    /// <summary>
    /// Lo usamos para coger referencia de todos los scripts que se 
    /// necesitan para guardar la partida y para activar los cheats.
    /// Y también para activar la primera música
    /// </summary>
    void Start()
    {
        AudioManager.Instance.PlayMusic("savannahMusic");

        _playerController = FindFirstObjectByType<PlayerController>();
        _abilitiesManager = FindFirstObjectByType<AbilitiesManager>();
        _timer = FindFirstObjectByType<Timer>();
        _health = FindFirstObjectByType<Health>();
        _capture = FindFirstObjectByType<Capture>();
        _gun = FindFirstObjectByType<GrapplerGun>();
        _inventoryController = FindFirstObjectByType<InventoryController>();
        _checkList = FindFirstObjectByType<CheckList>();
        GameManager.Instance.Variables(_playerController, _abilitiesManager, _timer, _health, _capture, _gun, _inventoryController, _checkList);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static LevelManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el LevelManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // De momento no hay nada que inicializar
    }


    #endregion
} // class LevelManager 
// namespace