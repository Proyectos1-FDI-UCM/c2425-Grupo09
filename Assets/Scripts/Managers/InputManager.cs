//---------------------------------------------------------
// Contiene el componente de InputManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín, Sergio Gonzalez Lopez
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Manager para la gestión del Input. Se encarga de centralizar la gestión
/// de los controles del juego. Es un singleton que sobrevive entre
/// escenas.
/// La configuración de qué controles realizan qué acciones se hace a través
/// del asset llamado InputActionSettings que está en la carpeta Settings.
/// 
/// A modo de ejemplo, este InputManager tiene métodos para consultar
/// el estado de dos acciones:
/// - Move: Permite acceder a un Vector2D llamado MovementVector que representa
/// el estado de la acción Move (que se puede realizar con el joystick izquierdo
/// del gamepad, con los cursores...)
/// - Fire: Se proporcionan 3 métodos (FireIsPressed, FireWasPressedThisFrame
/// y FireWasReleasedThisFrame) para conocer el estado de la acción Fire (que se
/// puede realizar con la tecla Space, con el botón Sur del gamepad...)
///
/// Dependiendo de los botones que se quieran añadir, será necesario ampliar este
/// InputManager. Para ello:
/// - Revisar lo que se hace en Init para crear nuevas acciones
/// - Añadir nuevos métodos para acceder al estado que estemos interesados
///  
/// </summary>
public class InputManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static InputManager _instance;

    /// <summary>
    /// Controlador de las acciones del Input. Es una instancia del asset de 
    /// InputAction que se puede configurar desde el editor y que está en
    /// la carpeta Settings
    /// </summary>
    private InputActionSettings _theController;
    
    /// <summary>
    /// Acción para Fire. Si tenemos más botones tendremos que crear más
    /// acciones como esta (y crear los métodos que necesitemos para
    /// conocer el estado del botón)
    /// </summary>
    private InputAction _fire;
    private InputAction _jump;
    private InputAction _heal;
    private InputAction _capture;
    private InputAction _grappler;
    private InputAction _tiger;
    private InputAction _shield;
    private InputAction _checklist;
    private InputAction _exit;
    private InputAction _testing;
    private InputAction _map;
    private InputAction _save;
    private InputAction _load;
    private InputAction _play;
    private InputAction _pause;
    private InputAction _closeMap;
    private InputAction _closePauseMenu;
    private InputAction _closeChecklist;
    private InputAction _cheats;
    private InputAction _nextTutorial;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// 
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // InputManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Nos destruímos. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer InputManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
    } // Awake

    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
    } // OnDestroy

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static InputManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    } // Instance

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>True si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Propiedad para acceder al vector de movimiento.
    /// Según está configurado el InputActionController,
    /// es un vector normalizado 
    /// </summary>
    public Vector2 MovementVector { get; private set; }
    public Vector2 MapMovementVector { get; private set; }

    /// <summary>
    /// Los dos métodos sirven para activar unos controles u otros
    /// Los cotroles de la UI o los de Player
    /// </summary>
    public void EnableUIControls()
    {
        _theController.Player.Disable();
        _theController.UI.Enable();
    }

    public void EnablePlayerControls()
    {
        _theController.UI.Disable();
        _theController.Player.Enable();
    }
    public void DisablePlayerControls() 
    { 
        _theController?.Player.Disable();
    }
    
    /// <summary>
    /// Método para saber si el botón de disparo (Fire) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool FireIsPressed()
    {
        return _fire.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de disparo (Fire) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool FireWasPressedThisFrame()
    {
        return _fire.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de disparo (Fire) ha dejado de pulsarse
    /// durante este frame
    /// <returns>Devuelve true, si el botón se ha dejado de pulsar en
    /// este frame; y false, en otro caso.
    /// </returns>
    /// </summary>
    public bool FireWasReleasedThisFrame()
    {
        return _fire.WasReleasedThisFrame();
    }

    public bool JumpWasPressedThisFrame()
    {
        return _jump.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de curacion (Heal) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool HealWasPressedThisFrame()
    {
        return _heal.WasPressedThisFrame();
    }

    public bool HealWasReleasedThisFrame()
    {
        return _heal.WasReleasedThisFrame();
    }

    public bool HealIsPressed()
    {
        return _heal.IsPressed();
    }

    public bool CaptureWasPressedThisFrame()
    {
        return _capture.WasPressedThisFrame();
    }
    public bool TigerWasPressedThisFrame()
    {
        return _tiger.WasPressedThisFrame();
    }
    public bool TigerIsPressed()
    {
        return _tiger.IsPressed();
    }

    public bool GrapplerWasPressedThisFrame()
    {
        return _grappler.WasPressedThisFrame();
    }

    public bool GrapplerWasReleasedThisFrame()
    {
        return _grappler.WasReleasedThisFrame();
    }

    public bool GrapplerIsPressed()
    {
        return _grappler.IsPressed();
    }

    public bool ShieldWasPressedThisFrame()
    {
        return _shield.WasPressedThisFrame();
    }

    public bool ChecklistWasPressedThisFrame()
    {
        return _checklist.WasPressedThisFrame();
    }

    public bool TestingWasPressedThisFrame()
    {
        return _testing.WasPressedThisFrame();
    }

    public bool ExitWasPressedThisFrame()
    {
        return _exit.WasPressedThisFrame();
    }

    public bool MapWasPressedThisFrame()
    {
        return _map.WasPressedThisFrame();
    }

    public bool SaveWasPressedThisFrame()
    {
        return _save.WasPressedThisFrame();
    }

    public bool LoadWasPressedThisFrame()
    {
        return _load.WasPressedThisFrame();
    }
    public bool PlayWasPressedThisFrame()
    {
        return _play.WasPressedThisFrame();
    }
    public bool PauseWasPressedThisFrame()
    {
        return _pause.WasPressedThisFrame();
    }
    public bool CheatWasPressedThisFrame() 
    { 
        return _cheats.WasPressedThisFrame();
    }


    #region UI Methods
    public bool MapCloseWasPressedThisFrame()
    {
        return _closeMap.WasPressedThisFrame();
    }

    public bool PauseMenuCloseWasPressedThisFrame()
    {
        return _closePauseMenu.WasPressedThisFrame();
    }

    public bool ChecklistCLoseWasPressedThisFrame()
    {
        return _closeChecklist.WasPressedThisFrame();
    }

    public bool NextTutorialWasPressedThisFrame()
    {
        return _nextTutorial.WasPressedThisFrame();
    }
    #endregion


    //Esto es para detectar si hay mando o no

    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    public bool MandoConectado()
    {
        // Comprobar si hay al menos un mando conectado
        if (Gamepad.current != null)
        {
            Debug.Log("Hay un mando conectado");
            return true;
            // Mostrar HUD de mando
        }

        return false;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    Debug.Log("Mando conectado");
                    if(HUDAbilities.Instance != null)
                    HUDAbilities.Instance.UpdateHUDForGamePad(true);
                    break;
                case InputDeviceChange.Removed:
                    Debug.Log("Mando desconectado");
                    if(HUDAbilities.Instance != null)
                    HUDAbilities.Instance.UpdateHUDForGamePad(false);

                    break;
            }
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // Creamos el controlador del input y activamos los controles del jugador
        _theController = new InputActionSettings();
        _theController.Player.Enable();

        // Cacheamos la acción de movimiento
        InputAction movement = _theController.Player.Move;
        InputAction mapMovement = _theController.UI.Navigate;
        // Para el movimiento, actualizamos el vector de movimiento usando
        // el método OnMove
        movement.performed += OnMove;
        movement.canceled += OnMove;
        // Para el movimiento dentro del UI, actualizamos el vector de movimiento usando
        // el método OnMove
        mapMovement.performed += OnMoveMap;
        mapMovement.canceled += OnMoveMap;
        // Para el disparo solo cacheamos la acción de disparo.
        // El estado lo consultaremos a través de los métodos públicos que 
        // tenemos (FireIsPressed, FireWasPressedThisFrame 
        // y FireWasReleasedThisFrame)
        _fire = _theController.Player.Fire;
        _jump = _theController.Player.Jump;
        _heal = _theController.Player.Heal;
        _capture = _theController.Player.Capture;
        _grappler = _theController.Player.Grappler;
        _tiger = _theController.Player.Tiger;
        _shield = _theController.Player.Shield;
        _checklist = _theController.Player.CheckList;

        _testing = _theController.Player.Testing;
        _exit = _theController.Player.Exit;
        _map =_theController.Player.Map;
        _save = _theController.Player.Save;
        _load = _theController.Player.Load;
        _play = _theController.Player.Play;
        _pause = _theController.Player.Pause;
        _cheats = _theController.Player.Cheats;

        //UI Controls
        _closeMap = _theController.UI.MapClose;
        _closePauseMenu = _theController.UI.PauseMenuClose;
        _closeChecklist = _theController.UI.ChecklistClose;
        _nextTutorial = _theController.UI.Tutorial;

    }

    /// <summary>
    /// Método que es llamado por el controlador de input cuando se producen
    /// eventos de movimiento (relacionados con la acción Move)
    /// </summary>
    /// <param name="context">Información sobre el evento de movimiento</param>
    private void OnMove(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();
    }

    private void OnMoveMap(InputAction.CallbackContext context)
    {
        MapMovementVector = context.ReadValue<Vector2>();
    }

    #endregion
} // class InputManager 
// namespace