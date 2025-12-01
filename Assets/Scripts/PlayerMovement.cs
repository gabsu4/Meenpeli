using UnityEngine;
using UnityEngine.Timeline;
using System.Collections; // Tarvitaan Coroutinea varten

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private AudioClip[] Walk;
    [SerializeField] private float WalkInterval = 0.4f;
    [SerializeField] private float moveSpeed = 5f;
    private float nextStepTime = 0f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f; // Kuinka lujaa dash lähtee
    [SerializeField] private float dashDuration = 0.2f; // Kuinka kauan dash kestää (sekunteina)
    private float dashCooldown = 1.0f;
    private float nextDashTime = 0f;
    private bool isDashing = false; // Onko dash käynnissä

    [Header("Combat & Stats")]
    public Animator animator;
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    public bool KnockFromRight;
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
        // Varmistetaan GameManagerin olemassaolo ennen käyttöä
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
        if(isDead) return;

        // Jos dash on päällä, emme halua kääntää hahmoa tai lukea kävelysyötettä samalla tavalla
        if (isDashing) return;

        HandleDash(); // Tarkistetaan painoiko pelaaja dash-nappia
        HandleFootsteps();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack"); 
        }

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(inputX, inputY).normalized;
        Vector3 currentScale = transform.localScale;

        // Päivitetään viimeisin liikesuunta vain jos liikutaan.
        // Tämä varmistaa, että dash lähtee sinne minne viimeksi käveltiin, vaikka oltaisiin pysähdyksissä.
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
        if(isDead) return;

        // Jos dash on käynnissä, FixedUpdate ei saa puuttua nopeuteen.
        // DashCoroutine hoitaa liikkumisen tällä hetkellä.
        if (isDashing) return;

        if (animator != null)
        {
            animator.SetFloat("Speed", movement.magnitude);
        }

        // Knockback logiikka
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
        // Soitetaan ääniä vain jos liikutaan ja EI dashata
        if (rb.linearVelocity.magnitude > 0.1f && Time.time >= nextStepTime && !isDashing)
        {
            PlayRandomFootstep();
            nextStepTime = Time.time + WalkInterval; 
        }
    }

    private void PlayRandomFootstep()
    {
        if (Walk == null || Walk.Length == 0) return;

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
            // Käynnistetään dash-aliohjelma (Coroutine)
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true; // Estää normaalin liikkumisen Updatessa ja FixedUpdatessa
        nextDashTime = Time.time + dashCooldown;

        // Varmistetaan että lastMoveDir ei ole nolla (ettei jäädä paikalleen)
        Vector2 dashDirection = lastMoveDir == Vector3.zero ? new Vector2(transform.localScale.x, 0) : (Vector2)lastMoveDir;

        // Asetetaan kova vauhti suoraan Rigidbodyyn
        rb.linearVelocity = dashDirection.normalized * dashSpeed;

        // Odotetaan dashin keston ajan
        yield return new WaitForSeconds(dashDuration);

        // Dash loppui, palautetaan kontrolli
        rb.linearVelocity = Vector2.zero; // Pysäytetään liike hetkeksi (valinnainen, tekee dashista napakamman)
        isDashing = false;
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
        if (isDead) return; 

        isDead = true; 
        animator.SetTrigger("Die");

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }
}