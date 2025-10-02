using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 10; // Hahmon maksimi HP
    
    // Yksityinen muuttuja, joka seuraa nykyistä HP:ta
    private int currentHealth;

    private void Start()
    {
        // Asetetaan nykyinen terveys maksimitasolle pelin alussa
        currentHealth = maxHealth;
        Debug.Log($"Pelaajan HP: {currentHealth}/{maxHealth}");
    }

    /// <summary>
    /// Funktio, jota kutsutaan, kun hahmo ottaa vahinkoa.
    /// </summary>
    /// <param name="damageAmount">Otettava vahingon määrä.</param>
    public void TakeDamage(int damageAmount)
    {
        // Vähennetään nykyistä terveyttä
        currentHealth -= damageAmount;

        // Varmistetaan, ettei HP mene nollan alle ennen kuolemaa
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log($"Pelaaja otti {damageAmount} DMG:tä. Jäljellä HP: {currentHealth}/{maxHealth}");

        // Tarkistetaan, onko hahmon HP nolla tai alle
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Funktio, joka suoritetaan hahmon kuollessa.
    /// </summary>
    private void Die()
    {
        Debug.Log("Pelaaja kuoli!");
        
        // TÄHÄN TULEE KUOLEMALOGIIKKA:
        
        // 1. Pysäytä peli (tai näytä Game Over -ruutu)
        // Time.timeScale = 0f; 
        
        // 2. Piilota tai tuhoa hahmo
        Destroy(gameObject); 
        
        // 3. Tai vain deaktivoi se:
        // gameObject.SetActive(false);
    }
}