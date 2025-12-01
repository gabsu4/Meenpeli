using System;
using UnityEngine;

public class BossLifeCycle : MonoBehaviour
{
    // --- Public & Serialize Fields ---
    public Animator animator;
    public BossAiChase aiChaseScript; // Drag the BossAiChase script component here
    public BossDamage bossDamageScript; // Drag the BossDamage script component here
    public Rigidbody2D rb; // Drag the Rigidbody2D component here
    
    [SerializeField] private AudioClip DeadSound;
    private float soundVolumeBoost = 0.5f;

    // --- Health Variables ---
    public int maxHealth = 100;
    public int currentHealth;
    public bool IsDead { get; private set; } = false;

    // --- Phase Two Logic (Kept for integration) ---
    public event Action OnHealthThresholdReached;
    private const int PhaseTwoThreshold = 50; // Example Phase 2 HP
    private bool phaseTwoTriggered = false;


    void Awake()
    {
        // Safety checks for component assignment (in case they are missed in the Inspector)
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (aiChaseScript == null) aiChaseScript = GetComponent<BossAiChase>();
        if (bossDamageScript == null) bossDamageScript = GetComponent<BossDamage>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        animator.SetTrigger("Hurt"); // Assuming you still have a Hurt trigger

        // Phase Two Check
        if (!phaseTwoTriggered && currentHealth <= PhaseTwoThreshold)
        {
            phaseTwoTriggered = true;
            OnHealthThresholdReached?.Invoke(); // Triggers the StartPhaseTwo in BossAiChase
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

        // 1. Play Visuals and Audio
        if (animator != null)
        {
            animator.SetBool("IsDead", true); 
        }
        // if (DeadSound != null)
        // {
        //     AudioHelper.PlayClip2D(DeadSound, transform.position, soundVolumeBoost);
        // }
        
        // 2. IMMEDIATE SHUTDOWN (Prevents movement/attack resurrection)
        
        // Stop Movement
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic; // Lock position
        }

        // Disable AI and Attacks
        if (aiChaseScript != null)
        {
            aiChaseScript.enabled = false;
        }
        if (bossDamageScript != null)
        {
            bossDamageScript.enabled = false;
        }
        
        // Disable main Collider
        GetComponent<Collider2D>().enabled = false;

        // Stop this script's Update/events if you decide to keep it enabled for a bit
        this.enabled = false; 
    }

    // This function MUST be called via an Animation Event on the death animation's last frame.
    public void FinalDeathCleanup()
    {
        // This permanently removes the boss from the scene.
        Destroy(gameObject); 
    }
}