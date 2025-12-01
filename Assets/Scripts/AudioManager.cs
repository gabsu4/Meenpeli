using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Yksittäinen instanssi (Singleton)
    public static AudioManager Instance;

    // Asetukset Unity Editorissa
    [Header("Mixer Settings")]
    public AudioMixer masterMixer; // Vedä MasterMixer tähän
    private const string MASTER_VOLUME_PARAM = "MasterVolume"; 
    
    [Header("Music Source")]
    public AudioSource musicSource; // Vedä saman GameObjectin AudioSource tähän

    [Header("Music Clips - Kytkennät Main Menussa")]
    public AudioClip menuMusic;
    public AudioClip tutorialMusic; 
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;
    public AudioClip bossMusic;
    
    // ---------------------------------------------------------

    void Awake()
    {
        // Varmistetaan, että vain yksi instanssi on olemassa
        if (Instance == null)
        {
            Instance = this;
            // TÄRKEÄ: Tämä pitää AudioManagerin hengissä scene-vaihtojen yli!
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
        // Ladataan tallennettu volyymi ja asetetaan se Mixeriin
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        SetMasterVolume(savedVolume);
        
        // Aloitetaan heti kuuntelemaan scene-vaihdoksia
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Soitetaan Main Menun musiikki, jos aloitetaan siellä
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            PlayMusic(menuMusic);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Musiikin vaihtaminen scenen latautumisen yhteydessä
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
                // Jos scene ei ole listassa, pidetään nykyinen musiikki tai musicSource.Stop();
                break;
        }
    }
    
    // Musiikin toistofunktio
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        
        // Estää saman musiikin uudelleenaloituksen
        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }
        
        musicSource.clip = clip;
        musicSource.Play();
    }

    // GLOBAALI VOLYYMIN SÄÄTÖFUNKTIO (kutsuu Audio Mixeriä)
    public void SetMasterVolume(float linearVolume)
    {
        // Muunnetaan lineaarinen volyymi (0..1) desibeli-asteikolle
        float dbVolume = Mathf.Log10(Mathf.Max(linearVolume, 0.0001f)) * 20f;

        if (masterMixer != null)
        {
            masterMixer.SetFloat(MASTER_VOLUME_PARAM, dbVolume);
        }
        
        // Tallennetaan arvo Slideria varten
        PlayerPrefs.SetFloat("masterVolume", linearVolume);
        PlayerPrefs.Save();
    }
}