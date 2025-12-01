using System.Collections;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    [SerializeField] private AudioClip hit;
    [SerializeField]private AudioClip playerDeathTaunt;
    public int attackDamage = 4;
    public Transform attackpoint;
    public LayerMask playerlayer;
    public float attackRange = 0.5f;
    public float attackCooldown = 1.5f;
    public float lastAttackTime = 0f;

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
        if (IsDead || Player == null)
        {
            return;
        }
    }

    public void StartAttack(Vector2 directionToPlayer)
    {
        if (IsAttacking) return;
    
        IsAttacking = true;
        lastAttackTime = Time.time; 

        bool isFacingBack = directionToPlayer.y > 0; 

        if (animator != null)
        {
            if (isFacingBack)
            {
                animator.SetTrigger("Attack_Back_Trigger"); 
            }
            else
            {
                animator.SetTrigger("Attack_Front_Trigger"); 
            }
        
            animator.SetBool("IsWalking", false); 
        }
    }

    public void PerformDamageHit()
    {   
        if (!IsAttacking || Player == null)
        {
            return;
        }

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, playerlayer);

        foreach (Collider2D playerCollider in hitPlayer)
        {
            if (playerCollider.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(attackDamage);
            }

            if (health != null && health.IsDead() && playerDeathTaunt != null) 
            {
                AudioHelper.PlayClip2D(playerDeathTaunt, transform.position, 1f); 
            }

            if (playerCollider.TryGetComponent<PlayerMovement>(out PlayerMovement movement))
            {
                movement.KBCounter = movement.KBTotalTime;
                movement.KnockFromRight = playerCollider.transform.position.x <= transform.position.x;
            }
        }
    
            if (hit != null)
                SoundManager.instance.PlaySound(hit);
    }

    public void EndAttack()
    {
        IsAttacking = false;
    }
    public void Die()
    {
        if (IsDead) return;
        IsDead = true;
        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        StopAllCoroutines();
    }

    public void PlayDeathTaunt()
    {
        if (playerDeathTaunt != null)
        {
            AudioHelper.PlayClip2D(playerDeathTaunt, transform.position, 1f); 
        }
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
