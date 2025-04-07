using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI TimerText;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject DefeatSign;



    [SerializeField] float timeSeg = 600f;
    public float SumaSeg = 5f;
    float RestaSeg = 5f;

    static float currenttime;
    static bool playing = true;
    public Animator animator;
    private Health _health;
    private Defeat _defeat;
   

    void Start()
    {
        _health = Player.GetComponent<Health>();
        _defeat = DefeatSign.GetComponent<Defeat>();
        playing = true;
        currenttime = timeSeg;
        animator.SetBool("1", true);
    }

    void Update()
    {
        if (playing)
        {
            currenttime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(currenttime / 60);
            int seconds = Mathf.FloorToInt(currenttime % 60);

            if (minutes <= 0 && seconds <= 0)
            {  
                minutes = 0;
                seconds = 0;
                PlayerDead();
                TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); 
                StopCounting();
                
            }
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (Keyboard.current.rKey.wasPressedThisFrame) { currenttime = currenttime + SumaSeg; }
            if (Keyboard.current.eKey.wasPressedThisFrame) { currenttime = currenttime - RestaSeg; }

            if (currenttime > (timeSeg / 3) * 2) { animator.SetBool("1", true); }
            //if (currenttime < (time / 3) * 2 && currenttime > (time / 3)) { animator.SetBool("2", true);  }
            //if (currenttime < (time / 3)) { animator.SetBool("3", true); }
           

        }
    }

    public void Reloj(float x)
    { 
        currenttime =  currenttime + x;
        timeSeg = timeSeg + x;
    }

    public void StopCounting()
    {
        playing = false;
        Debug.Log("Fin partida");
    }

    public float MinutesCount ()
    {
        int minutes = Mathf.FloorToInt((timeSeg - currenttime) / 60);
        return minutes;
    }
    public float SecondsCount()
    {

        int seconds = Mathf.FloorToInt((timeSeg - currenttime) % 60);
        return seconds;
    }

    #region Save and Load
    public void Save(ref TimerData data)
    {
        playing = false;
        data.timeAmount = currenttime;

    }

    public void Load(TimerData data) 
    {
        playing = true;
        currenttime = data.timeAmount;

    }

    #endregion
    private void PlayerDead()
    {
        _defeat.ShowDefeat();
        _health.Die();
    }
}
[System.Serializable]
public struct TimerData
{
    public float timeAmount;
 
}

