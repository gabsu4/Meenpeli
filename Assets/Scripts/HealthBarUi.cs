using UnityEngine;
using UnityEngine.UI;

public class HealthBarUi : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Health targetHealthScript;

    void Start()
    {
        if (targetHealthScript == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                targetHealthScript = player.GetComponent<Health>();
            }
        }
        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
        }

        if (targetHealthScript != null)
        {
            healthSlider.maxValue = targetHealthScript.maxHealth;
            targetHealthScript.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(targetHealthScript.health, targetHealthScript.maxHealth);
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthSlider.value = currentHealth;
    }

    private void OnDestroy()
    {
        if (targetHealthScript != null)
        {
            targetHealthScript.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
