//---------------------------------------------------------
// Este script hace que el timer se actualice correctamente
// Diego García , Valeria Espada
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

using System.Collections;

public class Timer : MonoBehaviour
{

    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject DefeatSign;

    [SerializeField] float timeSeg = 600f;
    [SerializeField] float SumaSeg = 5f;
    private float RestaSeg = 5f;

    static float currenttime;
    static bool playing = true;
    [SerializeField] private Animator animator;



    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)
    private Health _health;
    private Defeat _defeat;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    void Start()
    {
        //Esto está desactivado porque el tiempo se activa desde el TutorialManager
        //playing = true;
        currenttime = timeSeg;
        animator.SetBool("1", true);
        int minutes = Mathf.RoundToInt(currenttime / 60);
        int seconds = Mathf.FloorToInt(currenttime % 60);

        if (minutes <= 0 && seconds <= 0)
        {
            {
                minutes = 0;
                seconds = 0;
                PlayerDead();
                TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                StopCounting();


            }
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            // if (Keyboard.current.rKey.wasPressedThisFrame) { currenttime = currenttime + SumaSeg; }


            //if (currenttime < (time / 3)) { animator.SetBool("3", true); }
        }

        if (InputManager.Instance.TestingWasPressedThisFrame())
            if (InputManager.Instance.TestingWasPressedThisFrame())
            {
                currenttime += SumaSeg;
            }
    }




    #endregion
    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Se usa cuando el jugador coge un reloj, para aumentar el tiempo.
    /// </summary>
    public void Reloj(float x)
    {
        currenttime = currenttime + x;
        timeSeg = timeSeg + x;
    }

    /// <summary>
    /// Se usa para parar el temporizador.
    /// </summary>
    public void StopCounting()
    {
        playing = false;
        Debug.Log("Fin partida");
    }

    /// <summary>
    /// Se usa para empezar el temporizador.
    /// </summary>
    public void StartCounting()
    {
        playing = true;
        Debug.Log("Inicio partida");
    }

    /// <summary>
    /// Se usa para ir contando los minutos.
    /// </summary>
    public float MinutesCount()
    {
        int minutes = Mathf.FloorToInt((timeSeg - currenttime) / 60);
        return minutes;
    }
    /// <summary>
    /// Se usa para ir contando los segundos.
    /// </summary>
    public float SecondsCount()
    {
        int seconds = Mathf.FloorToInt((timeSeg - currenttime) % 60);
        return seconds;
    }

    /// <summary>
    /// Se usa para poner el tiempo infinito al activar los cheats.
    /// </summary>
    public void InfiniteTime()
    {
        TimerText.text = ("88:88");
    }

    #region Save and Load
    /// <summary>
    /// Guarda el tiempo que queda.
    /// </summary>
    public void Save(ref TimerData data)
    {
        playing = false;
        data.timeAmount = currenttime;

    }

    /// <summary>
    /// Carga el tiempo que queda.
    /// </summary>
    public void Load(TimerData data)
    {
        playing = true;
    }

#endregion

#endregion

// ---- MÉTODOS PRIVADOS ----

#region Métodos Privados
    /// <summary>
    /// Se llama cuando muere el jugador.
    /// </summary>
    private void PlayerDead()
    {
        _defeat.ShowDefeat();
        _health.Die();
    }
    #endregion
}

#region Struct SaveAndLoad
/// <summary>
/// En este struct se guarda el tiempo que queda.
/// </summary>
[System.Serializable]
public struct TimerData
{
    public float timeAmount;

}

#endregion