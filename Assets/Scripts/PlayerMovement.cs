using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerMovement : MonoBehaviour
{
    // Muuttuja hahmon nopeudelle. Näkyy ja säädettävissä Unityn Inspectorissa.
    [SerializeField] private float moveSpeed = 5f;

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
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        HandleDash();
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