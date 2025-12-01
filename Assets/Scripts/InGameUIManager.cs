using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    [Header("UI Containers")]
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject inventoryCanvas;
    
    [Header("Volume Controls")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;
    
    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Start()
    {
        optionsCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", 1f); 
        volumeSlider.value = savedVolume;
        UpdateVolumeText(savedVolume);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptions();
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    private void ToggleOptions()
    {
        bool isActive = optionsCanvas.activeSelf;
        optionsCanvas.SetActive(!isActive);
        
        Time.timeScale = isActive ? 1f : 0f;
    }

    private void ToggleInventory()
    {
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
    }

    public void SetVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(volume);
        }
        UpdateVolumeText(volume);
    }
    
    private void UpdateVolumeText(float volume)
    {
         if (volumeTextValue != null)
            volumeTextValue.text = volume.ToString("0.0");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(mainMenuSceneName);
    }
}