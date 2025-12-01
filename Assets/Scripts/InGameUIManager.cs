using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    // UI Panel Controllerin kentät (ESC ja TAB)
    [Header("UI Containers")]
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject inventoryCanvas;
    
    // Options Panel Controllerin kentät (Volyymi)
    [Header("Volume Controls")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;
    
    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Start()
    {
        // Varmistaa, että valikot ovat kiinni alussa
        optionsCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        
        // Asetetaan Sliderin kuuntelijat valmiiksi
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        
        // Ladataan tallennettu volyymi ja päivitetään Slider ja teksti
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", 1f); 
        volumeSlider.value = savedVolume;
        UpdateVolumeText(savedVolume);
    }

    void Update()
    {
        // ESC-Nappi: Options-valikon toggle
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptions();
        }

        // TAB-Nappi: Inventory-valikon toggle
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    private void ToggleOptions()
    {
        bool isActive = optionsCanvas.activeSelf;
        optionsCanvas.SetActive(!isActive);
        
        // TÄRKEÄÄ: Säädetään Time.timeScale!
        // Jos valikko avataan (isActive on false), pysäytetään aika.
        // Jos valikko suljetaan (isActive on true), aika palautetaan.
        Time.timeScale = isActive ? 1f : 0f;
    }

    private void ToggleInventory()
    {
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        // Huom: Inventaarion avaaminen ei yleensä pysäytä peliä, joten Time.timeScalea ei tarvita.
    }

    // KORJATTU VOLYYMIN SÄÄTÖ: Kutsuu AudioManageria
    public void SetVolume(float volume)
    {
        // Kutsuu globaalia, tuhoutumatonta AudioManageria
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
        // Varmistaa, että peli ei ole taukotilassa, kun sceneä ladataan
        Time.timeScale = 1f; 
        SceneManager.LoadScene(mainMenuSceneName);
    }
}