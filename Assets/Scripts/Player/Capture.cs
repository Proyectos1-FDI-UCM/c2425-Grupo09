//---------------------------------------------------------
// Script responsable de capturar y manejar los aspectos posteriores a cada captura de animal 
// Valeria Espada, Diego García, Alejandro Garcia
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor.Rendering;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] GameObject[] AnimalsList;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private bool _near = false;  
    private GameObject _animal;  
    private BarraDeSueño _barraDeSueño; 
    private PlayerController _playerController;
    private GrapplerGun _grapplerGun;
    private Health _health;
    private CheckList _checkList;
    private int _capturedAnimals = 0;
    private List <int> _animalCapture = new List<int>();

    private enum _animalIdentifier
    {
        MaleBunny,
        FemaleBunny,
        MaleBat,
        FemaleBat,
        MaleGorila,
        FemaleGorila,
        MaleTiger,
        FemaleTiger,
        MaleArmadillo,
        FemaleArmadillo,
        MaleDog,
        FemaleDog
    }

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _grapplerGun = GetComponentInChildren<GrapplerGun>();
        _health = GetComponent<Health>();
        _checkList = GetComponent<CheckList>();
        _capturedAnimals = 0;
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

            if (_near && InputManager.Instance.CaptureWasPressedThisFrame() && _barraDeSueño.Dormido())
            {
                Debug.Log("capturePressed");
                animator.SetTrigger("Capture");
                AudioManager.Instance.PlaySFX("pick", false);
                CheckpointManager.Instance.SetCheckpoint(transform.position);
                _capturedAnimals++;
                _animalCapture.Add(_barraDeSueño.AnimalId);
                Debug.Log("capturePressed");
                if (_animal.CompareTag("Bunny"))
                {
                    Debug.Log("saltos extra:" + _playerController.ExtraJump);
                    AbilitiesManager.Instance.BunnyAbilityUnlock();
                    _playerController.ExtraJump = 1;

                    if(_barraDeSueño.Male) _checkList.ActivateTick((int)_animalIdentifier.MaleBunny);
                    else _checkList.ActivateTick((int)_animalIdentifier.FemaleBunny);
                }
                else if (_animal.CompareTag("Bat"))
                {
                    Debug.Log("activada la vision nocturna");
                    AbilitiesManager.Instance.BatAbilityUnlock();
                    _playerController.nightVision.SetActive(true);

                    if (_barraDeSueño.Male) _checkList.ActivateTick((int)_animalIdentifier.MaleBat);
                    else _checkList.ActivateTick((int)_animalIdentifier.FemaleBat);
                }
                else if (_animal.CompareTag("Gorila"))
                {
                    AbilitiesManager.Instance.GorilaAbilityUnlock();
                    _grapplerGun.GrapplerUnlocked = true;

                    if (_barraDeSueño.Male) _checkList.ActivateTick((int)_animalIdentifier.MaleGorila);
                    else _checkList.ActivateTick((int)_animalIdentifier.FemaleGorila);
                }
                else if (_animal.CompareTag("Tiger"))
                {
                    AbilitiesManager.Instance.TigerAbilityUnlock();
                    _playerController.TigerUnlocked = true;

                    if (_barraDeSueño.Male) _checkList.ActivateTick((int)_animalIdentifier.MaleTiger);
                    else _checkList.ActivateTick((int)_animalIdentifier.FemaleTiger);
                }
                else if (_animal.CompareTag("Armadillo"))
                {
                    AbilitiesManager.Instance.ArmadilloAbilityUnlock();
                    _health.ArmadilloUnlocked = true;

                    if (_barraDeSueño.Male) _checkList.ActivateTick((int)_animalIdentifier.MaleArmadillo);
                    else _checkList.ActivateTick((int)_animalIdentifier.FemaleArmadillo);
                }
                else if (_animal.CompareTag("Dog"))
                {
                    AbilitiesManager.Instance.DogAbilityUnlock();
                    _playerController.DogUnlocked = true;

                    if (_barraDeSueño.Male) _checkList.ActivateTick((int)_animalIdentifier.MaleDog);
                    else _checkList.ActivateTick((int)_animalIdentifier.FemaleDog);
                }
                RecogerObjeto();
                Debug.Log("captureEnded");
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
    /// <summary>
    /// Se usa para saber el número de animales que ha capturado 
    /// una vez que el jugador ya ha acabado la partida.
    /// </summary>
    public int AnimalCount ()
    { return _capturedAnimals; }
    /// <summary>
    /// Se usa para activar las habilidades de los animales
    /// cuando se ponen los cheats.
    /// </summary>
    public void Cheats()
    {
        //Abilities
        _playerController.nightVision.SetActive(true);
        _health.ArmadilloUnlocked = true;
        _grapplerGun.GrapplerUnlocked = true;
        _playerController.ExtraJump = 1;
        _playerController.TigerUnlocked = true;
        _playerController.DogUnlocked = true;
        
        //Consumables
        _playerController.CoconutUnlocked = true;
        _playerController.BananaUnlocked = true;
        _playerController.SeedUnlocked = true;
    }

    #region Save and Load
    /// <summary>
    /// Se usa para guardar los animales capturados.
    /// </summary>
    public void Save(ref CaptureData data)
    {
        data.captures = _animalCapture;
    }
    /// <summary>
    /// Se usa para cargarr los animales capturados.
    /// </summary>
    public void Load( CaptureData data)
    {
         _animalCapture = data.captures;
        for (int i = 0; i < _animalCapture.Count; i++) 
        {
            Destroy(AnimalsList[_animalCapture[i]]);   
        }

    }
    #endregion

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
#region Struct SaveAndLoad
/// <summary>
/// Se usa para guardar los animales capturados.
/// </summary>
[System.Serializable]
public struct CaptureData
{
    public List<int> captures;
}
#endregion