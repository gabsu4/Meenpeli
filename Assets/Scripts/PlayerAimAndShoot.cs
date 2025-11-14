using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAimAndShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    private void Update()
    { 
        HandleGunShooting();
    }

    private void HandleGunShooting()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (worldPosition - (Vector2)bulletSpawnPoint.position).normalized;

            GameObject spawnedBullet = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);

            BulletBehavior bulletScript = spawnedBullet.GetComponent<BulletBehavior>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction, 15f);
            }

            Collider2D playerCollider = GetComponent<Collider2D>();
            Collider2D bulletCollider = spawnedBullet.GetComponent<Collider2D>();
            if (playerCollider != null && bulletCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
            }
         }
     }
}