using NUnit.Framework;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 10;
    public int currentHealth;
    public bool IsDead = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if (IsDead) return;
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        if (IsDead) return;
        IsDead = true;

        GetComponent<MonsterDamage>()?.Die();
        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        AiChase aiChase = GetComponent<AiChase>();
        if (aiChase != null)
        {
            aiChase.enabled = false;
        }
        this.enabled = false;
    }
}
