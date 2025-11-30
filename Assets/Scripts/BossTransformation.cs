using UnityEngine;

public class BossTransformation : MonoBehaviour
{
    private bool hasTransformed = false;
    private bool playerInRange = false;
    private BossAiChase bossAI;
    private Animator bossAnimator;
    private Collider2D interactionCollider;
    private bool arenaEntered = false;

    private const string TRANSFORM_TRIGGER = "Transform"; 

    void Start()
    {
        bossAI = GetComponent<BossAiChase>();
        bossAnimator = GetComponent<Animator>();
        interactionCollider = GetComponent<Collider2D>();
        if (bossAI != null)
        {
            bossAI.IsActive = false;
        }
        if (bossAnimator != null)
        {
            bossAnimator.Play("Boss_jorma"); 
        }
    }
    void Update()
    {
        if (arenaEntered && playerInRange && !hasTransformed && Input.GetKeyDown(KeyCode.E))
        {
            TransformIntoBoss();
        }
    }
    public void PlayerEnteredArena()
    {
        arenaEntered = true;
        if (interactionCollider != null)
        {
            interactionCollider.isTrigger = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    public void OnTransformationComplete()
    {
        if (bossAI == null) 
        {
            Debug.LogError("FATAL ERROR: BossTransformation failed to find BossAiChase component during activation! Re-checking...");
            bossAI = GetComponent<BossAiChase>(); 
        }
        
        if (hasTransformed && bossAI != null)
        {
            bossAI.ActivateBoss();
        }
    }

    private void TransformIntoBoss()
    {
        hasTransformed = true; 
        
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger(TRANSFORM_TRIGGER); 
        }
        if (interactionCollider != null)
        {
            interactionCollider.enabled = false;
        }
    }
}

