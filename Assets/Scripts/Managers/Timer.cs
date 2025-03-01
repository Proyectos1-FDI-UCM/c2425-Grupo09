using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI TimerText;

    [SerializeField] float timeSeg = 600f;
    [SerializeField] public float SumaSeg = 5f;
    [SerializeField] float RestaSeg = 5f;

    static float currenttime;
    static bool playing = true;
    public Animator animator;
   

    void Start()
    {
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
                TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); StopCounting();
            }
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (Keyboard.current.rKey.wasPressedThisFrame) { currenttime = currenttime + SumaSeg; }
            if (Keyboard.current.eKey.wasPressedThisFrame) { currenttime = currenttime - RestaSeg; }

            if (currenttime > (timeSeg / 3) * 2) { animator.SetBool("1", true); }
            //if (currenttime < (time / 3) * 2 && currenttime > (time / 3)) { animator.SetBool("2", true);  }
            //if (currenttime < (time / 3)) { animator.SetBool("3", true); }
           

        }
    }

    public void Reloj()
    { 
        currenttime =  currenttime + SumaSeg;
    }

    public static void StopCounting()
    {
        playing = false;
        Debug.Log("Fin partida");
    }
}

