using NUnit.Framework;
using UnityEngine;
using System;


public class EnemyHealth : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private AudioClip Dead;
    [SerializeField] private AudioClip[] Hurt;
    private float soundVolumeBoost = 0.5f;
    public event Action OnHealthThresholdReached;
    public int CurrentHealth => currentHealth;
    public int maxHealth = 10;
    public int currentHealth;
    public bool IsDead = false;
    private const int PhaseTwoThreshold = 25;
    private bool phaseTwoTriggered = false;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        if (currentHealth > 0 && Hurt.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, Hurt.Length);
        
            AudioClip randomClip = Hurt[randomIndex];

            AudioHelper.PlayClip2D(randomClip, transform.position, soundVolumeBoost);
        }
        animator.SetTrigger("Hurt");

        if (!phaseTwoTriggered && currentHealth <= PhaseTwoThreshold)
        {
            phaseTwoTriggered = true;
            OnHealthThresholdReached?.Invoke();
        }

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        if (IsDead) return;
        IsDead = true;

        if (Dead != null)
        {
            AudioHelper.PlayClip2D(Dead, transform.position, soundVolumeBoost);
        }

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
