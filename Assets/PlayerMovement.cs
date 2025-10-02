using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Dodge Settings")]
    [SerializeField] private float dodgeSpeed = 15f; // Kuinka nopeasti hahmo väistää
    [SerializeField] private float dodgeDuration = 0.15f; // Väistön kesto sekunneissa
    [SerializeField] private float dodgeCooldown = 1.0f; // Väistön jäähdytysaika

    // Viittaukset komponenteihin
    private Rigidbody2D rb;

    // Liikkeen muuttujat
    private Vector2 movement;

    // Väistön tilaa seuraavat muuttujat
    private bool isDodging = false;
    private float dodgeTimer = 0f;
    private float cooldownTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // --- 1. Jäähdytysajan hallinta ---
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // --- 2. Väistönhallinta (kun väistö on käynnissä) ---
        if (isDodging)
        {
            dodgeTimer -= Time.deltaTime;

            if (dodgeTimer <= 0)
            {
                isDodging = false;
                // Kun väistö loppuu, palataan normaaliin liikkumiseen FixedUpdate():ssa.
            }
            // Älä käsittele muuta syötettä väistön aikana
            return;
        }

        // --- 3. Liikesyötteen lukeminen ---
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // Luodaan liikesuunta ja normalisoidaan se (ei vinosti nopeammin)
        movement = new Vector2(inputX, inputY).normalized;

        // --- 4. Väistönapin käsittely ---
        // Tarkistetaan, painetaanko Space-näppäintä, ollaanko jäähdytysajan ulkopuolella,
        // ja onko hahmo liikkumassa johonkin suuntaan (movement.magnitude > 0)
        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0 && movement.magnitude > 0)
        {
            StartDodge();
        }
    }

    private void FixedUpdate()
    {
        if (isDodging)
        {
            // Väistön aikana käytetään tallennettua suuntaa * dodgeSpeed
            // Väistö on nopeampi ja "työntää" hahmoa
            rb.linearVelocity = movement * dodgeSpeed;
        }
        else
        {
            // Normaali liikkuminen
            rb.linearVelocity = movement * moveSpeed;
        }
    }

    private void StartDodge()
    {
        // Jos hahmo ei liiku (movement.magnitude on nolla), emme tee väistöä.
        if (movement.magnitude == 0) return;

        isDodging = true;
        dodgeTimer = dodgeDuration;
        cooldownTimer = dodgeCooldown;

        // Tässä vaiheessa *movement* sisältää jo sen suunnan, johon hahmo on juuri liikkunut.
        // FixedUpdate() käyttää tätä arvoa väistönopeudella.
    }
}