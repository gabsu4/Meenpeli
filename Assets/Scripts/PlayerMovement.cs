using UnityEditor.Experimental.GraphView;
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
        HandleDash();
        HandleFootsteps();
        float inputX = Input.GetAxisRaw("Horizontal"); // Oletuksena A/D tai nuolinäppäimet
        float inputY = Input.GetAxisRaw("Vertical");   // Oletuksena W/S tai nuolinäppäimet

        // Luodaan uusi Vector2, joka edustaa hahmon haluttua liikesuuntaa.
        movement = new Vector2(inputX, inputY).normalized;
        Vector3 currentScale = transform.localScale;

        if (movement.magnitude > 0.1f) // Only update if a direction is being held
        {
            lastMoveDir = movement;
        }

        if (Mathf.Abs(inputX) > 0.01f)
            animator.SetInteger("AnimState", 2);
        else
            animator.SetInteger("AnimState", 0);

        if (inputX > 0 && !facingRight)
            Flip();
        else if (inputX < 0 && facingRight)
            Flip();
        
    }
    private void FixedUpdate()
    {
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
    // Check if the player is moving AND if the sound is ready to play
    if (rb.linearVelocity.magnitude > 0.1f && Time.time >= nextStepTime)
    {
        PlayRandomFootstep();
        nextStepTime = Time.time + WalkInterval; // Reset the timer
    }
}

private void PlayRandomFootstep()
{
    // 1. Check if the array is valid and has clips
    if (Walk == null || Walk.Length == 0)
    {
        Debug.LogWarning("Footstep Sounds array is empty! Assign clips in the Inspector.");
        return;
    }

    // 2. Select a random index from 0 up to (but not including) the array length
    int randomIndex = Random.Range(0, Walk.Length);
    
    // 3. Get the random clip
    AudioClip randomClip = Walk[randomIndex];

    // 4. Play the sound using your SoundManager
    if (SoundManager.instance != null)
    {
        SoundManager.instance.PlaySound(randomClip);
    }
}

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float dashDistance = 5f;
            transform.position += lastMoveDir * dashDistance;
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}