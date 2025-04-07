//---------------------------------------------------------
// Script que se encarga de gestionar el audio del juego
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    public static AudioManager Instance { get; private set; } // Singleton

    [Header("Settings")]
    [SerializeField] private float MaxPitch = 1.1f;
    [SerializeField] private float MinPitch = 0.9f;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource RandomPitchSFXSource;

    [Header("Audio Clip")]
    [Header("Music")]
    public AudioClip background;
    public AudioClip mainMenu;
    public AudioClip victory;
    public AudioClip gameOver;
    [Header("Player SFX")]
    public AudioClip shoot;
    public AudioClip walk;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip playerHurt;
    public AudioClip[] die;
    public AudioClip pick;
    public AudioClip eat;
    public AudioClip shield;
    public AudioClip launchGrappler;
    public AudioClip swingGrappler;
    [Header("Animal SFX")]
    public AudioClip attack;
    public AudioClip animalJump;
    public AudioClip animalHurt;
    public AudioClip animalKO;
    public AudioClip bat;
    public AudioClip[] tiger;
    public AudioClip gorilla;
    public AudioClip gorillaJump;
    [Header("Interactuables SFX")]
    public AudioClip pickApple;
    public AudioClip pickTime;
    [Header("Oher SFX")]
    public AudioClip magicCharge;
    public AudioClip magicExplosion;
    public AudioClip magicTinkle;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public void PlayMusic(AudioClip clip)
    {
        if(clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        } else{
            Debug.LogWarning("Music clip " +  clip.name + " is null. Cannot play sound effect.");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// Reproduce un sonido
    /// </summary>
    /// <param name="clip">Audio que se quiere reproducir</param>
    /// <param name="randomPitch">true si se debe reproducir el sonido con un tono aleatorio, false si no</param>
    public void PlaySFX(AudioClip clip, bool randomPitch = false)
    {
        if(clip != null)
        {
            if(randomPitch)
            {
            RandomPitchSFXSource.pitch = Random.Range(MinPitch, MaxPitch);
            RandomPitchSFXSource.PlayOneShot(clip);
            }
            else
            {
                SFXSource.PlayOneShot(clip);
            }
        } else{
            Debug.LogWarning("Audio clip " +  clip.name + " is null. Cannot play sound effect.");
        }
        
    }

    /// <summary>
    /// Reproduce un sonido aleatorio de una lista de sonidos
    /// </summary>
    /// <param name="clips">Sonidos entre los que se selecciona uno aleatorio</param> 
    /// <param name="randomPitch">true si se debe reproducir el sonido con un tono aleatorio, false si no</param>
    public void PlayRandomSFX(AudioClip[] clips, bool randomPitch = false)  
    {
        int randomIndex = Random.Range(0, clips.Length);

        if(clips[randomIndex] != null)
        {
            if(randomPitch)
            {
                RandomPitchSFXSource.pitch = Random.Range(MinPitch, MaxPitch);
                RandomPitchSFXSource.PlayOneShot(clips[randomIndex]);
            }
            else
            {
                SFXSource.PlayOneShot(clips[randomIndex]);
            }
        } else{
            Debug.LogWarning("Audio clip array is null. Cannot play sound effect.");
        }
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados


    #endregion   

} // class AudioManager 
// namespace
