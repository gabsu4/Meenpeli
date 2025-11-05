using UnityEngine;

public class weaponGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform ammo;
    public float bulletSpeed = 10f;
    void Update()
    {
        AimAtMouse();
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    void AimAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos -transform.position).normalized;
        transform.up = direction;
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, ammo.position,ammo.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = ammo.up * bulletSpeed;
    }
}