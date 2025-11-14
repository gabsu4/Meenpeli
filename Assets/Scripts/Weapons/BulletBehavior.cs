using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float normalBulletSpeed = 15f;
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private LayerMask whatDestroysBullet;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetDestroyTime();
    }

    public void SetDirection(Vector2 dir, float speed)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.velocity = dir * speed;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            Destroy(gameObject);
        }
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }
}