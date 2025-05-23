//---------------------------------------------------------
// Este script se encarga de gestionar el sistema de checkpoints
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Clase que gestiona el sistema de checkpoints del juego.
/// </summary>
public class CheckpointManager : MonoBehaviour
{    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    public static CheckpointManager Instance { get; private set; }

    private Vector3 _lastCheckpoint;
    private Health _playerHealth;
    private GameObject _player;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Se llama desde el Capture cuando se recoge un animal, para establecer el checkpoint.
    /// </summary>
    public void SetCheckpoint(Vector3 checkPosition)
    {
        _lastCheckpoint = checkPosition;
    }
    /// <summary>
    /// Se usa para revivir al jugador despues de que haya muerto.
    /// </summary>
    public void Revivir()
    {
        StartCoroutine(ResetPlayer());
    }
    /// <summary>
    /// Se usa para coger la referencia y la posicion del jugador.
    /// </summary>
    public void PlayerReference(GameObject player)
    {
        _player = player;
        _playerHealth = _player.GetComponent<Health>();
        SetCheckpoint(_player.transform.position);
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    
    /// <summary>
    /// Resetea al jugador a la posición del último checkpoint.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetPlayer()
    {
        yield return new WaitForSeconds(1f);

        UIManager.Instance.FadeIn();

        _player.SetActive(false);
        _player.transform.position = _lastCheckpoint;

        yield return new WaitForSeconds(1f);
        _playerHealth.ResetPlayer();
        
        _player.SetActive(true);
        UIManager.Instance.FadeOut();
    }

    #endregion   

} // class CheckpointManager 
// namespace
