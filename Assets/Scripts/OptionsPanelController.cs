using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsPanelController : MonoBehaviour
{
    [Header("Volume Controls")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void OnEnable()
    {
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        volumeSlider.value = savedVolume;
        
        SetVolume(savedVolume);

        volumeSlider.onValueChanged.RemoveListener(SetVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void OnDisable()
    {
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }

        if (volumeTextValue != null)
            volumeTextValue.text = volume.ToString("0.0");

        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();
    }

    public void ReturnToMainMenu()
    {
        Debug.Log($"[Settings] Palataan Main Menuun. TimeScale asetettu: {Time.timeScale}");
        Debug.Log($"[Settings] Yritetään ladata kenttä: {mainMenuSceneName}");
        Time.timeScale = 1f; 
        
        SceneManager.LoadScene(mainMenuSceneName);
    }
}