using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAiChase : MonoBehaviour
{
    [SerializeField] private BossDamage bossDamage;
    public AudioClip First;
    private EnemyHealth enemyHealth;
    public Transform attackpoint;
    public GameObject player;
    public float speed;
    private float distance;
    private const float ChaseDistance = 40;
    public float chaseHoldTime = 0.5f;
    private Coroutine stopChase;

    public bool isPhaseTwo { get; private set; } = false;

    public Animator animator;
    private bool Voice = false;
    private float soundVolumeBoost = 0.5f;
    private Collider2D bossCollider;

    public bool IsActive { get; set; } = false;
    void Start()
    {
        if (enemyHealth == null) enemyHealth = GetComponent<EnemyHealth>();
        if (bossDamage == null) bossDamage = GetComponent<BossDamage>();
        if (animator == null) animator = GetComponent<Animator>();
        if (bossCollider == null) bossCollider = GetComponent<Collider2D>();

        if (bossDamage != null) bossDamage.enabled = false;
        
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthThresholdReached += StartPhaseTwo; 
        }

        if (bossCollider != null)
        {
            bossCollider.isTrigger = true;
        }
    }
    void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthThresholdReached -= StartPhaseTwo; 
        }
    }

    public void ActivateBoss() 
    {
        if (IsActive) return; 

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); 
        }

        if (bossCollider != null) 
        {
            if (bossCollider.isTrigger) 
            {
                bossCollider.isTrigger = false;
            }
        }

        IsActive = true;
        
        if (bossDamage != null)
        {
            bossDamage.enabled = true;
        }
    }

    void Update()
    {
        if (!IsActive || enemyHealth == null || enemyHealth.IsDead || player == null)
        {
            if (player == null) this.enabled = false;
            return;
        }

        bool currentlyAttacking = 
            bossDamage != null && bossDamage.enabled && bossDamage.IsAttacking;
        
        if(currentlyAttacking)
        {
            animator.SetBool("IsWalking", false);
            return; 
        }

        if (isPhaseTwo)
        {
            HandlePhaseTwoBehavior();
            return;
        }


        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < ChaseDistance)
        {
            if(distance <= bossDamage.attackRange)
            {
                if (player != null & !Voice) 
                {
                    PlayVoiceLine();
                    Voice = true; 
                }
                animator.SetBool("IsWalking", false);
                if (Time.time - bossDamage.lastAttackTime >= bossDamage.attackCooldown)
                {
                    Vector2 attackDirection = (player.transform.position - transform.position).normalized;
                    bossDamage.StartAttack(attackDirection);
                }
            }
            else if(distance > bossDamage.attackRange)
            {
                Vector2 targetPosition = player.transform.position;
                Vector2 currentPosition = transform.position;
        
                Vector2 moveDirection = (targetPosition - currentPosition).normalized;
        
                animator.SetBool("IsWalking", true);
                UpdateVisuals(moveDirection);

                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            
                if (stopChase != null)
                {
                    StopCoroutine(stopChase);
                    stopChase = null;
                }
            }
        }
        else
        {
            Voice = false;
            animator.SetBool("IsWalking", false);

            animator.SetFloat("X_Dir", 0.01f);
            animator.SetFloat("Y_Dir", 0.01f);

            if (stopChase == null)
            {
                stopChase = StartCoroutine(StopChasingAfterDelay());
            }
        }
    }

    private void UpdateVisuals(Vector2 intendedMovementDirection)
    {
        float horizontal = intendedMovementDirection.x;
        float vertical = intendedMovementDirection.y;

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical)) 
        {
            animator.SetFloat("X_Dir", horizontal);
            animator.SetFloat("Y_Dir", 0.01f); 
        }
        else
        {
            animator.SetFloat("X_Dir", 0.01f);
            animator.SetFloat("Y_Dir", vertical); 
        }
    }


    private void StartPhaseTwo()
    {
        if (isPhaseTwo) return;
        isPhaseTwo = true;

        if (stopChase != null)
        {
            StopCoroutine(stopChase);
            stopChase = null;
        }

        if (bossDamage != null)
        {
            bossDamage.enabled = false;
        }
    }
    private void HandlePhaseTwoBehavior()
    {
        if (player == null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetFloat("X_Dir", 0.01f);
            animator.SetFloat("Y_Dir", 0.01f);
            return;
        }   
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= bossDamage.attackRange)
        {
            animator.SetBool("IsWalking", false);
        
            if (Time.time - bossDamage.lastAttackTime >= bossDamage.attackCooldown)
            {
                Vector2 attackDirection = (player.transform.position - transform.position).normalized;
                bossDamage.StartAttack(attackDirection);
            }
        
            animator.SetFloat("X_Dir", 0.01f);
            animator.SetFloat("Y_Dir", 0.01f);
        }
        else 
        {
            Vector2 targetPosition = player.transform.position;
            Vector2 currentPosition = transform.position;
    
            Vector2 moveDirection = (targetPosition - currentPosition).normalized;
    
            animator.SetBool("IsWalking", true);
            UpdateVisuals(moveDirection);

            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        }
    }
    private IEnumerator StopChasingAfterDelay()
    {
        yield return new WaitForSeconds(chaseHoldTime);
        stopChase = null;
    }

    private void PlayVoiceLine()
    {
        if(First != null)
        {
            AudioHelper.PlayClip2D(First, transform.position, soundVolumeBoost);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackpoint != null && bossDamage != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackpoint.position, bossDamage.attackRange);
        }
    }
}
