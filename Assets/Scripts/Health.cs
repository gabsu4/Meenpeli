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

    private float soundVolumeBoost = 0.5f;
    private BossDamage bossDamage;

    public void TestHeal(int amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHealth);

        OnHealthChanged?.Invoke(health, maxHealth);
    
        Debug.Log("--- HEALTH.CS TESTI: Terveys parannettu R-näppäimellä: " + health); 
    }

    void Start()
    {
        health = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement == null)
        {
            playerMovement = GetComponentInParent<PlayerMovement>(); 
        }

        OnHealthChanged?.Invoke(health, maxHealth);

        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");
        if (bossObject != null)
        {
            bossDamage = bossObject.GetComponent<BossDamage>();
        }
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

            AudioHelper.PlayClip2D(randomClip, transform.position, soundVolumeBoost);
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
        if (bossDamage != null)
        {
            bossDamage.PlayDeathTaunt(); 
        }
        if (Die != null)
        {
            AudioHelper.PlayClip2D(Die, transform.position, soundVolumeBoost);
        }
        if (playerMovement != null)
        {
            playerMovement.Die();
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
    public bool IsDead()
    {
        return health <= 0;
    }
}
