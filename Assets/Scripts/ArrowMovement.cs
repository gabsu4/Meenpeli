using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector2 direction = transform.right;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 5f);
    }
}
