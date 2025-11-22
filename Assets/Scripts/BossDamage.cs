using UnityEngine;

public class BossDamage : MonoBehaviour
{
    [SerializeField] private AudioClip hit;
    public int attackDamage = 4;
    public Transform attackpoint;
    public LayerMask playerlayer;
    public float attackRange = 0.5f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    public Animator animator;
    public GameObject Player;
    private bool IsDead = false;
    public bool IsAttacking {get; private set; } = false;

    void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(Player == null)
        {
            return;
        }
        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                SoundManager.instance.PlaySound(hit);
                lastAttackTime = Time.time;
                Attack();
            }
        }
    }

    void Attack()
    {
        IsAttacking = true;
        if (animator != null)
            animator.SetTrigger("attack");

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, playerlayer);

        foreach (Collider2D Player in hitPlayer)
        {
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
    public void EndAttack()
    {
        IsAttacking = false;
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
