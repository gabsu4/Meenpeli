using System;
using UnityEngine;
using System.Collections;
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
    private Collider2D swordCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        swordCollider = GetComponent<Collider2D>();

        if (playerCollider != null && swordCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, swordCollider);
            swordCollider.enabled = false;
        }     
    }

    void Update()
    {
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

    public void EnableSwordCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }
    }
    public void DisableSwordCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);       
        }
        if (other.CompareTag ("Boss"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);       
        }
    }
}
//aika 8min https://www.youtube.com/watch?v=giJKCl-GVrU