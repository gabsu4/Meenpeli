using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Dodge Settings")]
    [SerializeField] private float dodgeSpeed = 15f; 
    [SerializeField] private float dodgeDuration = 0.15f; 
    [SerializeField] private float dodgeCooldown = 1.0f; 

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.3f; // Lyönnin kesto sekunneissa
    [SerializeField] private float attackCooldown = 0.5f; // Lyönnin jäähdytysaika

    private Rigidbody2D rb;

    // Tilan hallinta
    private Vector2 movement;
    private bool isDodging = false;
    private float dodgeTimer = 0f;
    private float cooldownTimer = 0f;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackCooldownTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
    }

    private void Update()
    {
        // --- 1. Ajastimien päivitys ---
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
        if (attackCooldownTimer > 0) attackCooldownTimer -= Time.deltaTime;

        // --- 2. Hyökkäyksen ajastin (Ei estä muuta toimintaa) ---
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            
            if (attackTimer <= 0)
            {
                EndAttack();
            }
        }

        // --- 3. Väistönhallinta (Estää muun toiminnan väistön ajan) ---
        if (isDodging)
        {
            dodgeTimer -= Time.deltaTime;

            if (dodgeTimer <= 0)
            {
                isDodging = false;
            }
            // Väistön aikana liikesyötettä ei lueta, vaan pidetään vanha suunta.
            return;
        }

        // --- 4. Liikesyötteen lukeminen ---
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(inputX, inputY).normalized;

        // --- 5. Hyökkäyssyötteen lukeminen ---
        // Hyökkäys sallittu, vaikka olisimme liikkumassa
        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0)
        {
            StartAttack();
        }

        // --- 6. Väistönapin käsittely ---
        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0 && movement.magnitude > 0)
        {
            StartDodge();
        }
    }

    private void FixedUpdate()
    {
        // Huom: TÄSSÄ KOHTAA EI OLE ENÄÄ ERILLISTÄ TARKISTUSTA isAttacking-tilalle.
        // Liike toimii aina normaalisti, ellei väistö ole käynnissä.

        if (isDodging)
        {
            rb.linearVelocity = movement * dodgeSpeed;
        }
        else
        {
            // Normaali liikkuminen (myös hyökkäyksen aikana)
            rb.linearVelocity = movement * moveSpeed;

            if (animator != null)
            {
                animator.SetFloat("Speed", movement.magnitude); 
            }
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

    private void StartAttack()
    {
        isAttacking = true;
        attackTimer = attackDuration;
        attackCooldownTimer = attackCooldown;

        // if (animator != null) animator.SetTrigger("Attack");

        // TÄMÄ ON KOHTA JOSSA VAHINKO TAI LYÖNTI VISUAALISET ALKAA
        Debug.Log("HAHMO LYÖ! (Voi liikkua samalla)"); 
    }

    private void EndAttack()
    {
        isAttacking = false;
    }
}