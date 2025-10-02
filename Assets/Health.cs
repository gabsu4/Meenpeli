using UnityEngine;
using UnityEngine.UI; // TÄRKEÄ: Tarvitaan Slider-komponentin käyttöön

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 10; 
    
    // UUSI: Julkinen muuttuja HP-palkin liittämistä varten
    [Header("UI Settings")]
    [SerializeField] private Slider healthBarSlider; 

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        
        // TÄRKEÄ UUSI KOHTA: Määritellään Sliderin maksimiarvo
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            // Asetetaan myös nykyinen arvo alussa
            healthBarSlider.value = currentHealth;
        }

        Debug.Log($"Pelaajan HP: {currentHealth}/{maxHealth}");
    }

    /// <summary>
    /// Funktio, jota kutsutaan, kun hahmo ottaa vahinkoa.
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        
        // UUSI KOHTA: Päivitetään Sliderin arvo
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        Debug.Log($"Pelaaja otti {damageAmount} DMG:tä. Jäljellä HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Pelaaja kuoli!");
        Destroy(gameObject); 
    }
}