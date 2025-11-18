using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private AudioClip Die;
    [SerializeField] private AudioClip[] Hurt;
    public int maxHealth = 10;
    public int health;
    
    void Start()
    {
        health = maxHealth;
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
        AudioSource.PlayClipAtPoint(Die, transform.position);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
        }
        Destroy(gameObject);
    }
}
