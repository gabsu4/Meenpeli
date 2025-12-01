using NUnit.Framework;
using UnityEngine;
using System;


public class EnemyHealth : MonoBehaviour
{
    public GameObject healthPotionPrefab;

    public Animator animator;
    [SerializeField] private AudioClip Dead;
    [SerializeField] private AudioClip[] Hurt;
    public AudioClip LowHp;
    public AudioClip NearDeathHp;
    public AudioClip PhaseHp;
    private float soundVolumeBoost = 0.5f;

    public event Action OnHealthThresholdReached;
    public int CurrentHealth => currentHealth;
    public int maxHealth = 10;
    public int currentHealth;
    public bool IsDead = false;

    private const int PhaseTwoThreshold = 25;
    private const int LowHpLine = 30;
    private const int NearDeathHpLine = 10;

    private bool phaseTwoTriggered = false;
    private bool lowHpTriggered = false; 
    private bool nearDeathHpTriggered = false;

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
            AudioHelper.PlayClip2D(PhaseHp, transform.position, soundVolumeBoost);
            OnHealthThresholdReached?.Invoke();
        }

        if (!lowHpTriggered && currentHealth <= LowHpLine)
        {
            lowHpTriggered = true;
            AudioHelper.PlayClip2D(LowHp, transform.position, soundVolumeBoost);
        }

        if (!nearDeathHpTriggered && currentHealth <= NearDeathHpLine)
        {
            nearDeathHpTriggered = true;
            AudioHelper.PlayClip2D(NearDeathHp, transform.position, soundVolumeBoost);
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

        DropLoot();

        GetComponent<MonsterDamage>()?.Die();
        AiChase aiChase = GetComponent<AiChase>();
        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }
        if (Dead != null)
        {
            AudioHelper.PlayClip2D(Dead, transform.position, soundVolumeBoost);
        }
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }

    private void DropLoot()
    {
        if (healthPotionPrefab != null && UnityEngine.Random.value < 1f)
        {
            Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
            Debug.Log(gameObject.name + " pudotti Health Potionin!");
        }
    }
}
