using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private AudioClip Die;
    private AudioSource audioSource;
    public int maxHealth = 10;
    public int health;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //audioSource.PlayOneShot(Die);
            //float soundDuration = Die.length;
            DiePlayer();
        }
    }
    private void DiePlayer()
    {
        // 1. Play the death sound at the player's position
        AudioSource.PlayClipAtPoint(Die, transform.position);

        // 2. Tell the persistent GameManager to end the game and show the UI
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
        }
    }
}
