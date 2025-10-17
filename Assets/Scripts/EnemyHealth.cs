using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 10;
    int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        GetComponent<MonsterDamage>()?.Die();
        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
