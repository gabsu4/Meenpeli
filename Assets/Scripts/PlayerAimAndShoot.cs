using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerAimAndShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    [SerializeField] private int maxClipSize = 10;
    [SerializeField] private int maxTotalAmmo = 100;
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private Key reloadKey = Key.R;

    private int currentClipAmmo;
    private int currentTotalAmmo;
    private bool isReloading = false;

    private void Awake()
    {
        currentClipAmmo = maxClipSize;
        currentTotalAmmo = maxTotalAmmo;
    }

    private void Update()
    { 
        HandleGunShooting();
        HandleReloadInput();
    }

    private void HandleGunShooting()
    {
        if (isReloading || currentClipAmmo <= 0)
        {
            return;
        }

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

            currentClipAmmo--;
        }
    }

    private void HandleReloadInput()
    {
        if (Keyboard.current[reloadKey].wasPressedThisFrame)
        {
            AttemptReload();
        }
    }

    public void AttemptReload()
    {
        if (isReloading) return;

        if (currentClipAmmo == maxClipSize || currentTotalAmmo == 0) return;

        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        
        yield return new WaitForSeconds(reloadTime);

        int neededAmmo = maxClipSize - currentClipAmmo;

        int ammoToTransfer = Mathf.Min(neededAmmo, currentTotalAmmo);

        currentClipAmmo += ammoToTransfer;
        currentTotalAmmo -= ammoToTransfer;

        isReloading = false;
    }
}