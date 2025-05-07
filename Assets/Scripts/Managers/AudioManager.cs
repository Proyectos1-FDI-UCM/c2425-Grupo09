//---------------------------------------------------------
// Script que se encarga de gestionar el audio del juego
// Sergio Valiente Urueña
// The Last Vessel
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    public static AudioManager Instance { get; private set; } // Singleton

    private Dictionary<string, AudioClip> audioClips;

    [Header("Settings")]
    [SerializeField] private float MaxPitch = 1.1f;
    [SerializeField] private float MinPitch = 0.9f;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource RandomPitchSFXSource;

    [Header("Audio Clip")]
    [Header("Music")]
    [SerializeField] private AudioClip savannahMusic;
    [SerializeField] private AudioClip caveMusic;
    [SerializeField] private AudioClip jungleMusic;
    [SerializeField] private AudioClip mainMenu;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip gameOver;
    [Header("Player SFX")]
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip walk;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip land;
    [SerializeField] private AudioClip playerHurt;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip pick;
    [SerializeField] private AudioClip eat;
    [SerializeField] private AudioClip shield;
    [SerializeField] private AudioClip launchGrappler;
    [SerializeField] private AudioClip swingGrappler;
    [Header("Animal SFX")]
    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip animalJump;
    [SerializeField] private AudioClip animalHurt;
    [SerializeField] private AudioClip animalKO;
    [SerializeField] private AudioClip bat;
    [SerializeField] private AudioClip tiger;
    [SerializeField] private AudioClip gorilla;
    [SerializeField] private AudioClip gorillaJump;
    [Header("Interactuables SFX")]
    [SerializeField] private AudioClip pickApple;
    [SerializeField] private AudioClip pickTime;
    [Header("Oher SFX")]
    [SerializeField] private AudioClip magicCharge;
    [SerializeField] private AudioClip magicExplosion;
    [SerializeField] private AudioClip magicTinkle;

    [Header("UI SFX")]
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip map;
    [SerializeField] private AudioClip checklist;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Metodos Monobehaviour
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

        audioClips = new Dictionary<string, AudioClip>
        {
            { "savannahMusic", savannahMusic },
            { "caveMusic", caveMusic },
            { "jungleMusic", jungleMusic },
            { "mainMenu", mainMenu },
            { "victory", victory },
            { "gameOver", gameOver },
            { "shoot", shoot },
            { "walk", walk },
            { "jump", jump },
            { "land", land },
            { "playerHurt", playerHurt },
            { "die", die },
            { "pick", pick },
            { "eat", eat },
            { "shield", shield },
            { "launchGrappler", launchGrappler },
            { "swingGrappler", swingGrappler },
            { "attack", attack },
            { "animalJump", animalJump },
            { "animalHurt", animalHurt },
            { "animalKO", animalKO },
            { "bat", bat },
            { "tiger", tiger },
            { "gorilla", gorilla },
            { "gorillaJump", gorillaJump },
            { "pickApple", pickApple },
            { "pickTime", pickTime },
            { "magicCharge", magicCharge },
            { "magicExplosion", magicExplosion },
            { "magicTinkle", magicTinkle },
            { "click", click },
            { "map", map },
            { "checklist", checklist }
        };
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public void PlayMusic(string audioClip)
    {
        AudioClip clip = audioClips[audioClip];

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
    public void PlaySFX(string audioClip, bool randomPitch = false)
    {
        AudioClip clip = audioClips[audioClip];

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


    #endregion

} // class AudioManager 
// namespace
