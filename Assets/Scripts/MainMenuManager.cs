using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager _;
    [SerializeField] private bool _debugMode;

    public enum MainMenuButtons { play, options, quit };
    public enum OptionsButtons { back };
    
    [Header("UI Containers")]
    [SerializeField] private GameObject _MainMenuContainer;
    [SerializeField] private GameObject _OptionsMenuContainer;
    
    [Header("Volume Control UI")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;
    
    // POISTETTU: [SerializeField] private AudioSource musicSource; 
    
    [Header("Scene Loading")]
    [SerializeField] private string _sceneToLoadAfterClickingPlay;

    public void Awake()
    {
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("There are more than 1 MainMenuManager's in the scene");
            Destroy(gameObject); // Tuhotaan ylimääräinen
        }
    }

    private void Start()
    {
        OpenMenu(_MainMenuContainer);

        // Asetetaan Sliderin arvo muistiin tallennettuun volyymiin
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", 1f); 
        volumeSlider.value = savedVolume;
        
        UpdateVolumeText(savedVolume);

        // Lisätään kuuntelija, joka kutsuu SetVolume-funktiota
        // HUOM: onValueChangedin on suositeltavaa olla kytkettynä myös Editorin puolella, jos AddListener ei ole käytössä
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }
    
    public void MainMenuButtonClicked(MainMenuButtons buttonClicked)
    {
        DebugMessage("Button Clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case MainMenuButtons.play:
                PlayClicked();
                break;
            case MainMenuButtons.options:
                OptionsClicked();
                break;
            case MainMenuButtons.quit:
                QuitGame();
                break;
            default:
                Debug.Log("Button clicked that wasn't implemented in MainMenuManager Method");
                break;
        }
    }

    public void OptionsClicked()
    {
        OpenMenu(_OptionsMenuContainer); 
    }
    
    public void ReturnToMainMenu()
    {
        OpenMenu(_MainMenuContainer);
    }
    
    public void OptionsButtonClicked(OptionsButtons buttonClicked)
    {
        switch (buttonClicked)
        {
            case OptionsButtons.back:
                ReturnToMainMenu();
                break;
        }
    }

    private void DebugMessage(string message)
    {
        if (_debugMode)
        {
            Debug.Log(message);
        }
    }

    public void PlayClicked()
    {
        SceneManager.LoadScene(_sceneToLoadAfterClickingPlay);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void OpenMenu(GameObject menuToOpen)
    {
        _MainMenuContainer.SetActive(menuToOpen == _MainMenuContainer);
        _OptionsMenuContainer.SetActive(menuToOpen == _OptionsMenuContainer);
    }

    private void UpdateVolumeText(float volume)
    {
         if (volumeTextValue != null)
            volumeTextValue.text = volume.ToString("0.0");
    }

    // KORJATTU: Kutsuu AudioManager-Singletonia volyymin säätöön
    public void SetVolume(float volume)
    {
        // 1. Kutsutaan globaalia AudioManager-Singletonia säätämään Mixeriä
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(volume);
        }
        else
        {
            Debug.LogWarning("AudioManager Instance not found! Volume is only visually updated.");
        }
        
        // 2. Päivitetään UI-teksti
        UpdateVolumeText(volume);
        
        // POISTETTU VANHA musicSource.volume = volume; JA PlayerPrefs tallennus
    }
}
