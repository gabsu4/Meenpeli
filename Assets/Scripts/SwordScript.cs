using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SwordScript : MonoBehaviour
{
    private Animator anim;
    public float meleeSpeed;
    public int damage;
    float timeUntilMelee;
    public Transform player;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(player.localScale.x) * Mathf.Abs(scale.x);
        transform.localScale = scale;

        transform.position = player.position;

        if (timeUntilMelee <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
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
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            Debug.Log("enemy hit");        
        }
    }
}
//aika 8min https://www.youtube.com/watch?v=giJKCl-GVrU