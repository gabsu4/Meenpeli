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
    private bool facingRight = false;

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
    }
    // FixedUpdate kutsutaan säännöllisin väliajoin ja on paras paikka fysiikkalaskelmille (kuten Rigidbodyjen liikuttamiseen).
    private void FixedUpdate()
    {
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
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}