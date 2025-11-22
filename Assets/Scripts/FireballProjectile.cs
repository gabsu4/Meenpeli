using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    public int damage = 5;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Health>(out Health playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }
        
        if (!other.CompareTag("Enemy"))
        {
             Destroy(gameObject);
        }
    }
}
