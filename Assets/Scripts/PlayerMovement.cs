using UnityEngine;
using UnityEngine.Timeline;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private AudioClip[] Walk;
    [SerializeField] private float WalkInterval = 0.4f;
    [SerializeField] private float moveSpeed = 5f;
    private float nextStepTime = 0f;

    public Animator animator;
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    public bool KnockFromRight;

    private float dashCooldown = 1.0f;
    private float nextDashTime = 0f;
    private bool isDead = false;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 lastMoveDir;
    private bool facingRight = false;
    public Collider2D playerCollider;

    void Start()
    {
        if (playerCollider != null && GetComponent<Collider2D>() != null)
        {
            Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>());
        }
        if (GameManager.Instance != null && GameManager.Instance.nextSpawnPosition != Vector2.zero)
        {
            transform.position = GameManager.Instance.nextSpawnPosition;
            GameManager.Instance.nextSpawnPosition = Vector2.zero; 
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(isDead)
            return;

        HandleDash();
        HandleFootsteps();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack"); 
        }

        float inputX = Input.GetAxisRaw("Horizontal"); // Oletuksena A/D tai nuolinäppäimet
        float inputY = Input.GetAxisRaw("Vertical");   // Oletuksena W/S tai nuolinäppäimet

        movement = new Vector2(inputX, inputY).normalized;
        Vector3 currentScale = transform.localScale;

        if (movement.magnitude > 0.1f) 
        {
            lastMoveDir = movement;
        }

        bool isMoving = movement.magnitude > 0.1f;
        animator.SetBool("IsWalking", isMoving);

        if (inputX > 0 && !facingRight)
            Flip();
        else if (inputX < 0 && facingRight)
            Flip();
        
    }
    private void FixedUpdate()
    {
        if(isDead)
            return;
        if (animator != null)
        {
            animator.SetFloat("Speed", movement.magnitude);
        }
        if (KBCounter <= 0)
        {
            rb.linearVelocity = movement * moveSpeed;
        }
        else
        {
            if (KnockFromRight == true)
            {
                rb.linearVelocity = new Vector2(-KBForce, KBForce);
            }
            else
            {
                rb.linearVelocity = new Vector2(KBForce, KBForce);
            }
            KBCounter -= Time.deltaTime;
        }

    }   
    
    private void HandleFootsteps()
{
    if (rb.linearVelocity.magnitude > 0.1f && Time.time >= nextStepTime)
    {
        PlayRandomFootstep();
        nextStepTime = Time.time + WalkInterval; 
    }
}

private void PlayRandomFootstep()
{
    if (Walk == null || Walk.Length == 0)
    {
        return;
    }

    int randomIndex = Random.Range(0, Walk.Length);
    
    AudioClip randomClip = Walk[randomIndex];

    if (SoundManager.instance != null)
    {
        SoundManager.instance.PlaySound(randomClip);
    }
}

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextDashTime)
        {
            float dashDistance = 5f;
            transform.position += lastMoveDir * dashDistance;
            nextDashTime = Time.time + dashCooldown;
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    public void Die()
    {
        if (isDead)
            return; 

        isDead = true; 

        animator.SetTrigger("Die");

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; 
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }
}