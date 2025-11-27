using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action<int, int> OnHealthChanged;

    [SerializeField] private AudioClip Die;
    [SerializeField] private AudioClip[] Hurt;
    public int maxHealth = 50;
    public int health;
    private PlayerMovement playerMovement;
    
    void Start()
    {
        health = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();

        OnHealthChanged?.Invoke(health, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        health = Mathf.Max(health, 0);

        OnHealthChanged?.Invoke(health, maxHealth);

        if (health > 0 && Hurt.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, Hurt.Length);
        
            AudioClip randomClip = Hurt[randomIndex];

            AudioSource.PlayClipAtPoint(randomClip, transform.position);
        }
        if (health <= 0)
        {
            DiePlayer();
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHealth);

        OnHealthChanged?.Invoke(health, maxHealth);
    }

    private void DiePlayer()
    {
        if (Die != null)
        {
            AudioSource.PlayClipAtPoint(Die, transform.position);
        }
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
        }
        foreach (var col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }
    }
}
