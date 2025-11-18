using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MouseSensitivityOptions : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityValueText;

    public static float MouseSensitivity = 1f;

    void Start()
    {
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
        sensitivitySlider.value = MouseSensitivity;

        UpdateSensitivityText(MouseSensitivity);

        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }
    private void OnSensitivityChanged(float value)
    {
        MouseSensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
        UpdateSensitivityText(value);
    }
    private void UpdateSensitivityText(float value)
    {
        if (sensitivityValueText != null)
            sensitivityValueText.text = value.ToString("F1");
    }
}
