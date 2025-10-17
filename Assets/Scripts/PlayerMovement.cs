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

    // Viittaus Rigidbody2D-komponenttiin
    private Rigidbody2D rb;

    // Muuttuja liikesuunnan tallentamiseen
    private Vector2 movement;

    // Awake kutsutaan kun skripti luodaan, ennen Startia.
    private void Awake()
    {
        // Haetaan Rigidbody2D-komponentti, joka on samassa GameObjectissa.
        rb = GetComponent<Rigidbody2D>();
    }

    // Update kutsutaan kerran per ruutu. Käytetään käyttäjän syötteen lukemiseen.
    private void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal"); // Oletuksena A/D tai nuolinäppäimet
        float inputY = Input.GetAxisRaw("Vertical");   // Oletuksena W/S tai nuolinäppäimet

        // Luodaan uusi Vector2, joka edustaa hahmon haluttua liikesuuntaa.
        movement = new Vector2(inputX, inputY).normalized;
        Vector3 currentScale = transform.localScale;

            if (inputX > 0)
                currentScale.x = -Mathf.Abs(currentScale.x); // face right
            else if (inputX < 0)
                currentScale.x = Mathf.Abs(currentScale.x); // face left

            transform.localScale = currentScale;
            
            if (Mathf.Abs(inputX) > Mathf.Epsilon)
            animator.SetInteger("AnimState", 2);
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
            if (KnockFromRight == false)
            {
                rb.linearVelocity = new Vector2(KBForce, KBForce);
            }

            KBCounter -= Time.deltaTime;
        }
    }
}