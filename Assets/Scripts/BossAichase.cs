using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAiChase : MonoBehaviour
{
    [SerializeField] private BossDamage bossDamage;
    private BossFireballAttack fireballAttack;
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

    public bool IsActive { get; set; } = false;
    private const int DIR_FRONT = 0;
    private const int DIR_BACK = 1;
    private const int DIR_RIGHT = 2;
    private const int DIR_LEFT = 3;
    

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        bossDamage = GetComponent<BossDamage>();
        fireballAttack = GetComponent<BossFireballAttack>();
        if (bossDamage != null) bossDamage.enabled = false;
        if (fireballAttack != null) fireballAttack.enabled = false;

        if (enemyHealth != null)
        {
            enemyHealth.OnHealthThresholdReached += StartPhaseTwo; 
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

        if (bossDamage == null) bossDamage = GetComponent<BossDamage>();
        if (fireballAttack == null) fireballAttack = GetComponent<BossFireballAttack>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); 
        }
        IsActive = true;
        
        if (bossDamage != null)
        {
            bossDamage.enabled = true;
        }
        if (fireballAttack != null)
        {
            fireballAttack.enabled = false;
        }
    }

    void Update()
    {
        Debug.Log($"AI Status: IsActive={IsActive}, Player={player}");
        if (!IsActive)
        {
            return;
        }
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            return;
        }

        if(player == null)
        {
            this.enabled = false;
            return;
        }

        bool currentlyAttacking = 
            (bossDamage != null && bossDamage.enabled && bossDamage.IsAttacking) ||
            (fireballAttack != null && fireballAttack.enabled && fireballAttack.IsAttacking);
        
        if(currentlyAttacking)
        {
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

            if(distance <= bossDamage.attackRange && !Voice)
            {
                if (player != null) 
                {
                    PlayVoiceLine();
                    Voice = true; 
                }
                animator.SetBool("IsWalking", false);
            }
            else if(distance > bossDamage.attackRange)
            {
                Vector2 targetPosition = player.transform.position;
                Vector2 currentPosition = transform.position;
        
                Vector2 moveDirection = (targetPosition - currentPosition).normalized;
                Debug.Log("CHASING: Setting IsWalking to TRUE and moving.");
        
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
        Debug.Log($"H: {horizontal:F2}, V: {vertical:F2} | Abs(H) > Abs(V): {Mathf.Abs(horizontal) > Mathf.Abs(vertical)}");
        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            if (horizontal > 0)
            {
                animator.SetInteger("Direction", DIR_RIGHT);
            }
            else
            {
                animator.SetInteger("Direction", DIR_LEFT);
            }
        }
        else
        {
        
            if (vertical > 0)
            {
                animator.SetInteger("Direction", DIR_BACK);
            }
            else
            {
                animator.SetInteger("Direction", DIR_FRONT);
            }
        }
    }


    private void StartPhaseTwo()
    {
        isPhaseTwo = true;
        Debug.Log("Phase two active");

        if (stopChase != null)
        {
            StopCoroutine(stopChase);
            stopChase = null;
        }

        if (bossDamage != null)
        {
            bossDamage.enabled = false;
        }

        if (fireballAttack != null)
        {
            fireballAttack.enabled = true;
        }
    }
    private void HandlePhaseTwoBehavior()
    {
        if (player == null)
        {
            animator.SetBool("IsWalking", false);
            return;
        }   
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > fireballAttack.shootingRange)
    {
        Vector2 targetPosition = player.transform.position;
        Vector2 currentPosition = transform.position;
        
        Vector2 moveDirection = (targetPosition - currentPosition).normalized;
        
        animator.SetBool("IsWalking", true);
        UpdateVisuals(moveDirection);

        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
    }
    else 
    {
        animator.SetBool("IsWalking", false);
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
