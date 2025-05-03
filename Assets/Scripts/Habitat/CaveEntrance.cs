//---------------------------------------------------------
// Script que maneja la entrada y salida de la cueva
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;



/// <summary>
/// Esta clase sirve para optimizar el juego, desactivando algunos objetos como los fondos o los animales cuando no se ven,
/// y activar el Glow del HUD del murciélago.
/// </summary>
public class CaveEntrance : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("Referencias Sabana")]
    [SerializeField] GameObject SavannahBackground;
    [SerializeField] GameObject SavannahAnimals;

    [Header("Referencias Cueva")]
    [SerializeField] GameObject CaveBackground;
    [SerializeField] GameObject CaveAnimals;

    [Header("Referencias Jungla")]
    [SerializeField] GameObject JungleBackground;
    [SerializeField] GameObject JungleAnimals;
    //Bool para saber si el jugador entra desde la izquierda o desde la derecha
    [SerializeField] bool enteringFromLeft;
    [SerializeField] private TextMeshProUGUI CaveText;



    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Collider2D _coll;

    private bool _caveUnlock;
    private bool _inCaveEntrance;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    

    /// <summary>
    /// Obtenemos el collider de la barrera
    /// </summary>
    void Start()
    {
        _coll = GetComponent<Collider2D>();

        _caveUnlock = false;
        _inCaveEntrance = false; 
        CaveText.gameObject.SetActive(false);
    }

    void Update()
    {
        Debug.Log("Cueva desbloqueada: " + _caveUnlock);
        Debug.Log("Entrada cueva: " + _inCaveEntrance);

        if (!_caveUnlock && _inCaveEntrance)
        {
            CaveText.gameObject.SetActive(true);
        }
        else
            CaveText.gameObject.SetActive(false);


    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método que se llama desde el AbilityManager cuando se desbloquea el murciélago para quitar la barrera de la cueva
    /// </summary>
    public void UnlockCave()
    {
        _coll.isTrigger = true;
        _caveUnlock = true;
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Calculamos si el jugador está entrando o saliendo de la cueva. En caso de que entre activamos el Glow del HUD, y los animales y fondos.
    /// En caso contrario los desactivamos.
    /// </summary>
    /// <param name="other"> Objeto que sale del trigger </param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            _inCaveEntrance = false;
            CheckpointManager.Instance.SetCheckpoint(other.transform.position);
            
            Vector2 exitDirection = (other.transform.position - _coll.bounds.center).normalized;

            bool entering = (exitDirection.x > 0 == enteringFromLeft);

            if(entering)
            {
                EnteringCave(true);
            } 
            else 
            {
                EnteringCave(false);

                //Si sale en dirección a la sabana, activamos sus objetos
                if(enteringFromLeft)
                {
                    SavannahBackground.SetActive(true);
                    SavannahAnimals.SetActive(true);
                }
                else
                {
                    JungleBackground.SetActive(true);
                    JungleAnimals.SetActive(true);
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            _inCaveEntrance = true;
        }
    }

    /// <summary>
    /// Método auxiliar que se encarga de activar o desactivar los objetos cuando el jugador entra o sale de la cueva.
    /// </summary>
    /// <param name="entering"> bool para saber si el jugador entra o sale </param>
    private void EnteringCave(bool entering)
    {
        HUDAbilities.Instance.BatGlow(entering);

        CaveAnimals.SetActive(entering);
        CaveBackground.SetActive(entering);

        if(entering)
        {
            SavannahBackground.SetActive(false);
            SavannahAnimals.SetActive(false);

            JungleBackground.SetActive(false);
            JungleAnimals.SetActive(false);
        }
    }


    #endregion   

} // class CaveEntrance 
// namespace
