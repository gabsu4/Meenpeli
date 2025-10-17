using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int attackDamage = 4;
    public Transform attackpoint;
    public LayerMask playerlayer;
    public float attackRange = 0.5f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    public Animator animator;
    public GameObject Player;
    private bool IsDead = false;

    void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (IsDead)
        {
            Debug.Log("Enemy is dead — won't attack.");
            return;
        }

        if (Player == null)
        {
            Debug.Log("Player reference is missing!");
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        Debug.Log("Distance to player: " + distanceToPlayer);

        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Player is within attack range");

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Debug.Log("Attack cooldown ready — attacking!");
                lastAttackTime = Time.time;
                Attack();
            }
        }
    }

    void Attack()
    {
        Debug.Log("Enemy attacking");
        if (animator != null)
            animator.SetTrigger("Attack");

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, playerlayer);

        foreach (Collider2D Player in hitPlayer)
        {
            Debug.Log("Player hit!");
            if (Player.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(attackDamage);
            }

            if (Player.TryGetComponent<PlayerMovement>(out PlayerMovement movement))
            {
                movement.KBCounter = movement.KBTotalTime;
                movement.KnockFromRight = Player.transform.position.x <= transform.position.x;
            }
        }
    }

    public void Die()
    {
        IsDead = true;
        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackpoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackpoint.position, attackRange);
        }
    }
}