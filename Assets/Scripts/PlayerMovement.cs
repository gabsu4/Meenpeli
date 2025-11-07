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

    public float dodgeSpeed = 15f; 
    public float dodgeDuration = 0.15f;
    public float dodgeCooldown = 1.0f; 
    
    // Tilan hallinta
    public bool isDodging = false;
    public float dodgeTimer = 0f;
    public float cooldownTimer = 0f;

    private Rigidbody2D rb;
    private Vector2 movement;
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
        float inputX = Input.GetAxisRaw("Horizontal"); // Oletuksena A/D tai nuolinäppäimet
        float inputY = Input.GetAxisRaw("Vertical");   // Oletuksena W/S tai nuolinäppäimet

        // Luodaan uusi Vector2, joka edustaa hahmon haluttua liikesuuntaa.
        movement = new Vector2(inputX, inputY).normalized;
        Vector3 currentScale = transform.localScale;

        if (Mathf.Abs(inputX) > 0.01f)
            animator.SetInteger("AnimState", 2);
        else
            animator.SetInteger("AnimState", 0);
        
        if (inputX > 0 && !facingRight)
            Flip();
        else if (inputX < 0 && facingRight)
            Flip();

        if (isDodging)
        {
            if (dodgeTimer <= 0)
            {
                isDodging = false;
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0 && movement.magnitude > 0)
        {
            StartDodge();
        }
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    // FixedUpdate kutsutaan säännöllisin väliajoin ja on paras paikka fysiikkalaskelmille (kuten Rigidbodyjen liikuttamiseen).
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
        else if (isDodging)
        {
            rb.linearVelocity = movement * dodgeSpeed;
        }
        else
        {
            // Normaali liikkuminen (myös hyökkäyksen aikana)
            rb.linearVelocity = movement * moveSpeed;
        }
    }   
    
    private void StartDodge()
    {
        if (movement.magnitude == 0) return;

        isDodging = true;
        dodgeTimer = dodgeDuration;
        cooldownTimer = dodgeCooldown;

        // if (animator != null) animator.SetTrigger("Dodge");
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}