using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage;
    public Health playerHealth;
    public PlayerMovement playerMovement;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMovement.KBCounter = playerMovement.KBTotalTime;
            if(collision.transform.position.x <= transform.position.x){
                playerMovement.KnockFromRight = true;
            }
            if(collision.transform.position.x > transform.position.x){
                playerMovement.KnockFromRight = false;
            }
            playerHealth.TakeDamage(damage);
        }
    }
}
