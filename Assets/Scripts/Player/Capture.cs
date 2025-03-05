//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

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
    [SerializeField] private GameObject jugador;  // Referencia al objeto del jugador en la escena.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private bool _near = false;  
    private GameObject _animal;  
    private BarraDeSueño _barraDeSueño; 

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start se llama cuando el script se habilita, antes de que se ejecute cualquier Update por primera vez.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Update se llama cada frame si el MonoBehaviour está habilitado.
    /// </summary>
    void Update()
    {
        if (_animal != null)
        {
            float distancia = Vector3.Distance(jugador.transform.position, _animal.transform.position);

            if (distancia <= distanciaMaxima)
            {
                _near = true;
            }
            else
            {
                _near = false;
            }

            if (_near && Input.GetKeyDown(KeyCode.F))
                //&& _barraDeSueño.Dormido())
            {
                RecogerObjeto();
            }
        }
        Debug.Log(_animal);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BarraDeSueño>() != null)
        {
            _animal = other.gameObject; 
            _barraDeSueño = _animal.GetComponent<BarraDeSueño>();
        }
    }

    /// <summary>
    /// Método llamado cuando el jugador sale de la zona de colisión.
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BarraDeSueño>() != null)
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

