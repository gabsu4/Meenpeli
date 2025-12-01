using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsPanelController : MonoBehaviour
{
    // Aseta nämä viitteet Unity Inspectorissa Options-canvasissa
    [Header("Volume Controls")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource musicSource; // HUOM: Musiikin AudioSource, johon volyymi vaikuttaa

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void OnEnable()
    {
        // 1. Aseta Sliderin arvo tallennettuun volyymiin
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        volumeSlider.value = savedVolume;
        
        // 2. Päivitä volyymi välittömästi (esim. jos musicSource on paikka, johon halutaan vaikuttaa)
        SetVolume(savedVolume);

        // 3. Lisää kuuntelija (listener) Sliderin arvon muuttuessa
        // Huom: onValueChanged pitää lisätä jokaisen kerran, kun Canvas aktivoidaan, 
        // tai vain kerran esim. Awake/Start. Tässä käytetään OnEnablea, koska 
        // tämä on paneeli, joka aktivoituu ja deaktivoituu.
        // Pura vanhat kuuntelijat ensin, jotta niitä ei tule useita (hyvä käytäntö)
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void OnDisable()
    {
        
        // Pura kuuntelija, jotta ei synny virheitä, jos slider tuhoutuu
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        // Aseta volyymi AudioSourceen (tämän pitäisi olla musiikin tai pää-AudioListenerin)
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }

        // Päivitä liukusäätimen teksti
        if (volumeTextValue != null)
            volumeTextValue.text = volume.ToString("0.0");

        // Tallenna uusi volyymi PlayerPrefseihin
        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();
    }

    public void ReturnToMainMenu()
    {
        Debug.Log($"[Settings] Palataan Main Menuun. TimeScale asetettu: {Time.timeScale}");
        Debug.Log($"[Settings] Yritetään ladata kenttä: {mainMenuSceneName}");
        Time.timeScale = 1f; 
        
        // Lataa Main Menu -kenttä
        SceneManager.LoadScene(mainMenuSceneName);
    }
}