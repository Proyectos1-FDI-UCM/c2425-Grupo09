//---------------------------------------------------------
// Este script se encarga de gestionar el sistema de checkpoints
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Fade;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    public static CheckpointManager Instance { get; private set; }

    private Vector3 _lastCheckpoint;

    private Animator _playerAnim;
    private Animator _fadeAnim;
    private Health _playerHealth;

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
    
    void Start()
    {
        _playerHealth = Player.GetComponent<Health>();
        _playerAnim = Player.GetComponent<Animator>();
        _fadeAnim = Fade.GetComponent<Animator>();

        SetCheckpoint(Player.transform.position);
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

    public void Revivir()
    {
        StartCoroutine(ResetPlayer());
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private IEnumerator ResetPlayer()
    {
        yield return new WaitForSeconds(1f);

        Fade.SetActive(true);

        Player.SetActive(false);
        Player.transform.position = _lastCheckpoint;

        yield return new WaitForSeconds(1f);
        _playerHealth._currentHealth = 100f;
        _playerHealth.Updatehealth(0f);
        
        Player.SetActive(true);
        _fadeAnim.SetTrigger("FadeOut");

        yield return new WaitForSeconds(0.4f);

        Fade.SetActive(false);
    }

    #endregion   

} // class CheckpointManager 
// namespace
