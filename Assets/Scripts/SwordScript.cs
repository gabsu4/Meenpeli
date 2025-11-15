using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SwordScript : MonoBehaviour
{
    [SerializeField] private AudioClip Lyönti;
    private Animator anim;
    public float meleeSpeed;
    public int damage;
    float timeUntilMelee;
    public Transform player;


    void Start()
    {
        anim = GetComponent<Animator>();
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Collider2D swordCollider = GetComponent<Collider2D>();
        if (playerCollider != null && swordCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, swordCollider);
        }
        else
        {
            Debug.LogWarning("Missing collider on player or sword!");
        }      
    }

    void Update()
    {
       // Vector3 scale = transform.localScale;
       // scale.x = Mathf.Sign(player.localScale.x) * Mathf.Abs(scale.x);
       // transform.localScale = scale;

       // transform.position = player.position;

        if (timeUntilMelee <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound(Lyönti);
            }
                anim.SetTrigger("Attack");
                timeUntilMelee = meleeSpeed;
            }
        }
        else
        {
            timeUntilMelee -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);       
        }
    }
}
//aika 8min https://www.youtube.com/watch?v=giJKCl-GVrU