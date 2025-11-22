using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAiChase : MonoBehaviour
{
    [SerializeField] private BossDamage bossDamage;
    private EnemyHealth enemyHealth;
    public Transform attackpoint;
    public GameObject player;
    public float speed;
    private float distance;
    private const float ChaseDistance = 15;
    public float chaseHoldTime = 0.5f;
    private Coroutine stopChase;

    private BossFireballAttack fireballAttack;
    public bool isPhaseTwo { get; private set; } = false;

    public Animator animator;
    private bool isFacingRight = true;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        if (bossDamage == null)
        {
            bossDamage = GetComponent<BossDamage>();
        }
        fireballAttack = GetComponent<BossFireballAttack>();
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

    void Update()
    {
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            return;
        }

        if(player == null)
        {
            SetRunningAnimation(false);
            this.enabled = false;
            return;
        }

        if(bossDamage != null && bossDamage.IsAttacking)
        {
            SetRunningAnimation(false);
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
            if(distance > bossDamage.attackRange)
            {
                SetRunningAnimation(true);
                FlipEnemy();

                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

                if (stopChase != null)
                {
                    StopCoroutine(stopChase);
                    stopChase = null;
                }
            }
            else
            {
                SetRunningAnimation(false);
            }
        }
        else
        {
            if (stopChase == null)
            {
                stopChase = StartCoroutine(StopChasingAfterDelay());
            }
        }
    }

    private void StartPhaseTwo()
    {
        isPhaseTwo = true;
        Debug.Log("Phase two active");

        SetRunningAnimation(false);
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
        FlipEnemy();
        SetRunningAnimation(false);
    }
    private IEnumerator StopChasingAfterDelay()
    {
        yield return new WaitForSeconds(chaseHoldTime);

        SetRunningAnimation(false);
        stopChase = null;
    }
    private void SetRunningAnimation(bool isRunning)
    {
        if (animator != null)
        {
            animator.SetBool("IsRunning", isRunning); 
        }
    }
    private void FlipEnemy()
    {
        float playerX = player.transform.position.x;
        float enemyX = transform.position.x;

        const float Flipping = 0.05f;

        if (playerX > enemyX + Flipping && !isFacingRight)//oikee flip
        {
            Flip();
        }
        else if (playerX < enemyX - Flipping && isFacingRight)//vasen flip
        {
            Flip();
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1f; 

        transform.localScale = localScale;
    }

    void OnDrawGizmosSelected()
    {
        if (attackpoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackpoint.position, bossDamage.attackRange);
        }
    }
}
