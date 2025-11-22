using UnityEngine;

public class BossFireballAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform shootPoint; 
    public GameObject player;
    
    [Header("Attack Settings")]
    public float attackCooldown = 3f;
    public float shootingRange = 10f; 
    public float fireballSpeed = 8f;

    private float lastAttackTime = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        this.enabled = false; 
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= shootingRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                ShootFireball();
            }
        }
    }

    void ShootFireball()
    {
        if (animator != null)
        {
            animator.SetTrigger("FireballAttack");
        }
        
        SpawnFireball();
    }
    
    public void SpawnFireball()
    {
        if (fireballPrefab == null || shootPoint == null)
        {
            Debug.LogError("Fireball Prefab or Shoot Point not assigned!");
            return;
        }
        
        GameObject fireball = Instantiate(fireballPrefab, shootPoint.position, Quaternion.identity);

        Vector2 direction = (player.transform.position - shootPoint.position).normalized;
        
        if (fireball.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = direction * fireballSpeed;
        }
        
        // Optional: Rotate the fireball to face the direction of travel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

