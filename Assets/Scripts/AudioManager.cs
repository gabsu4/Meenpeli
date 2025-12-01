using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer Settings")]
    public AudioMixer masterMixer;
    private const string MASTER_VOLUME_PARAM = "MasterVolume"; 
    
    [Header("Music Source")]
    public AudioSource musicSource;

    [Header("Music Clips - Kytkennät Main Menussa")]
    public AudioClip menuMusic;
    public AudioClip tutorialMusic; 
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;
    public AudioClip bossMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return;
        }
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        SetMasterVolume(savedVolume);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            PlayMusic(menuMusic);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Main Menu": 
                PlayMusic(menuMusic);
                break;
            case "LevelTutoriaali":
                PlayMusic(tutorialMusic);
                break;
            case "level1 1":
                PlayMusic(level1Music);
                break;
            case "Level2":
                PlayMusic(level2Music);
                break;
            case "Level3":
                PlayMusic(level3Music);
                break;
            case "levelBoss":
                PlayMusic(bossMusic);
                break;
            default:
                break;
        }
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        
        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }
        
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void SetMasterVolume(float linearVolume)
    {
        float dbVolume = Mathf.Log10(Mathf.Max(linearVolume, 0.0001f)) * 20f;

        if (masterMixer != null)
        {
            masterMixer.SetFloat(MASTER_VOLUME_PARAM, dbVolume);
        }
        
        PlayerPrefs.SetFloat("masterVolume", linearVolume);
        PlayerPrefs.Save();
    }
}