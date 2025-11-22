using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private AudioClip Die;
    [SerializeField] private AudioClip[] Hurt;
    public int maxHealth = 50;
    public int health;
    private PlayerMovement playerMovement;
    
    void Start()
    {
        health = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health > 0 && Hurt.Length > 0)
    {
        int randomIndex = Random.Range(0, Hurt.Length);
        
        AudioClip randomClip = Hurt[randomIndex];

        AudioSource.PlayClipAtPoint(randomClip, transform.position);
    }
        if (health <= 0)
        {
            DiePlayer();
        }
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
