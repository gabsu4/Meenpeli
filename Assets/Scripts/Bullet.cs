using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private AudioClip bow;
    public float speed;
    public float lifeTime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid; 
    public GameObject destroyEffect;
    void Start()
    {
        SoundManager.instance.PlaySound(bow);
        Invoke("DestroyBullet", lifeTime);
    }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy must take damage");
                hitInfo.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            if (hitInfo.collider.CompareTag("Boss"))
            {
                Debug.Log("Enemy must take damage");
                hitInfo.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            DestroyBullet();
        }

        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
    }
    void DestroyBullet()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
