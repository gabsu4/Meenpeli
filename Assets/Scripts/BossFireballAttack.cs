using UnityEngine;

public class BossFireballAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform shootPoint; 
    public GameObject player;
    public bool IsAttacking { get; private set; } = false;
    
    [Header("Attack Settings")]
    public float attackCooldown = 3f;
    public float shootingRange = 10f; 
    public float fireballSpeed = 8f;
    private float lastAttackTime = 0f;
    private Animator animator;

    private EnemyHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        this.enabled = false; 
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            this.enabled = false; 
            return;
        }

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
        IsAttacking = true;
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
            return;
        }
        
        GameObject fireball = Instantiate(fireballPrefab, shootPoint.position, Quaternion.identity);

        Vector2 direction = (player.transform.position - shootPoint.position).normalized;
        
        if (fireball.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = direction * fireballSpeed;
        }
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float adjustedAngle = angle + 90f;
        fireball.transform.rotation = Quaternion.Euler(0, 0, adjustedAngle);
    }
    public void EndAttack()
    {
        IsAttacking = false;
    }



    void OnDrawGizmosSelected()
    {
        if (shootPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(shootPoint.position, shootingRange);
        }
    }
}

