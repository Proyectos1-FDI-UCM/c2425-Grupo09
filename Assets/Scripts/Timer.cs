using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI TimerText;

    [SerializeField] float time = 600f;
    static float currenttime;
    static bool playing = true;
    public Animator animator;

   

    void Start()
    {
        playing = true;
        currenttime = time;
        animator.SetBool("1", true);
    }

    void Update()
    {
        if (playing)
        {
            currenttime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(currenttime / 60);
            int seconds = Mathf.FloorToInt(currenttime % 60);
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (currenttime > (time / 3) * 2) { animator.SetBool("1", true); }
            if (currenttime < (time / 3) * 2 && currenttime > (time / 3)) { animator.SetBool("2", true);  }
            //if (currenttime < (time / 3)) { animator.SetBool("3", true); }
            if (minutes == 0 && seconds == 0) { StopCounting(); }

        }
    }

    public static void StopCounting()
    {
        playing = false;
        Debug.Log("Fin partida");
    }
}

