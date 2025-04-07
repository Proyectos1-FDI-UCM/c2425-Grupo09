//---------------------------------------------------------
// Breve descripción del contenido del archivo:
// Este script gestiona la interacción de un objeto de tipo "Reloj de Arena" en el juego,
// el cual, al colisionar con un objeto que tenga el componente "PlayerController",
// ejecuta un método en el temporizador y luego destruye el objeto "Reloj de Arena".
// Responsable de la creación de este archivo: The Last Vessel
// Proyecto 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Esta clase representa un reloj de arena que interactúa con el jugador. Cuando el jugador
/// colisiona con el reloj de arena, se activa el temporizador y luego el reloj de arena se destruye.
/// </summary>
public class Clock : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (Serialized Fields)


    [SerializeField] float SumaSeg = 20f;
    [SerializeField] int ItemId;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (Private Fields)
    private Timer timer;
    private InventoryController inventoryController;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se llama al inicializar el script. Se utiliza para obtener el componente Timer
    /// desde el objeto "timerText".
    /// </summary>
    void Awake()
    {
        // Se obtiene el componente Timer del objeto asociado a "timerText"
        timer = FindFirstObjectByType<Timer>();
        inventoryController = FindFirstObjectByType<InventoryController>();
    }

    /// <summary>
    /// Start se llama en el primer fotograma cuando el script se habilita.
    /// Se utiliza para inicializar cualquier comportamiento necesario al inicio del juego.
    /// </summary>
    void Start()
    {
        // Aquí se pueden añadir inicializaciones adicionales si es necesario
    }

    /// <summary>
    /// Update se llama una vez por cada fotograma mientras el MonoBehaviour está habilitado.
    /// Se puede utilizar para actualizaciones continuas en el juego.
    /// </summary>
    void Update()
    {
        // Lógica a ejecutar cada fotograma
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Se llama cuando otro objeto con un Collider2D entra en contacto con el objeto.
    /// Si el objeto que entra en contacto tiene el componente "PlayerController", 
    /// se ejecuta el método Reloj del Timer y luego se destruye el objeto "Reloj de Arena".
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto colisionado tiene el componente "PlayerController"
        GameObject player = collision.gameObject;
        if (player.GetComponent<PlayerController>() != null)
        {
            inventoryController.InventoryAdd(ItemId);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pickTime);
            gameObject.SetActive(false);
            timer.Reloj(SumaSeg);
            Destroy(gameObject);

        }
    }

    #endregion
}


