using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChase : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    public Transform target;
    public Transform attackpoint;
    public GameObject player;
    public float speed;
    private float distance;

    public int damage;
    public float attackRange;
    public float attackDelay;
    private float lastAttackTime;
    public Animator animator;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
            if (enemyHealth != null && enemyHealth.IsDead)
        {
            return;
        }
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        if (distance < 6)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }

        //attacking
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer < attackRange)
        {
            if (Time.time > lastAttackTime + attackDelay)
            {
                target.SendMessage("TakeDamage", damage);
                lastAttackTime = Time.time;
            }
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
