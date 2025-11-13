using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChase : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    public Transform attackpoint;
    public GameObject player;
    public float speed;
    private float distance;
    private const float ChaseDistance = 15;
    public float chaseHoldTime = 0.5f;
    private Coroutine stopChase;

    public float attackRange;
    public Animator animator;
    private bool isFacingRight = true;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            return;
        }
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        if (distance < ChaseDistance)
        {
            // 1. Player is in range: Start/Continue running, cancel any pending stop command
            SetRunningAnimation(true);
            
            // If the coroutine is running, stop it because we've re-acquired the target
            if (stopChase != null)
            {
                StopCoroutine(stopChase);
                stopChase = null;
            }

            // 2. Movement and Orientation
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            FlipEnemy();
        }
        else
        {
            // 3. Player is out of range: Check if the stop routine has been started
            if (stopChase == null)
            {
                // Start the routine to delay the stop
                stopChase = StartCoroutine(StopChasingAfterDelay());
            }
            // IMPORTANT: The enemy continues to move and run while the coroutine is counting down.
            // If you want the movement to stop, you'd need a separate condition.
        }
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

        if (playerX > enemyX && !isFacingRight)//oikee flip
        {
            Flip();
        }
        else if (playerX < enemyX && isFacingRight)//vasen flip
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
            Gizmos.DrawWireSphere(attackpoint.position, attackRange);
        }
    }
}
