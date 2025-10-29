using UnityEngine;

public class weaponAmmo : MonoBehaviour
{
    public float lifetime = 2f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
