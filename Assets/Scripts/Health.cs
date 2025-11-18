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
            AudioSource.PlayClipAtPoint(Die, transform.position);
            //audioSource.PlayOneShot(Die);
            //float soundDuration = Die.length;
            Destroy(gameObject);
        }
    }
}
