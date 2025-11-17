using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAimAndShoot : MonoBehaviour
{
    private GunItemData gunData;

    private int currentClipAmmo;
    private int currentTotalAmmo;
    private bool isReloading;

    public Transform bulletSpawnPoint;

    public void SetGun(GunItemData data)
    {
        gunData = data;

        if (gunData == null)
        {
            enabled = false;
            return;
        }

        currentClipAmmo = gunData.maxClipSize;
        currentTotalAmmo = gunData.maxTotalAmmo;
        enabled = true;
    }

    private void Update()
    {
        if (gunData == null) return;

        HandleGunShooting();
        HandleReloadInput();
    }

    private void HandleGunShooting()
    {
        if (isReloading || currentClipAmmo <= 0) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 dir = (worldPos - (Vector2)bulletSpawnPoint.position).normalized;

            GameObject bullet = Instantiate(gunData.bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<BulletBehavior>().SetDirection(dir, 15f);

            currentClipAmmo--;
        }
    }

    private void HandleReloadInput()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
            AttemptReload();
    }

    public void AttemptReload()
    {
        if (isReloading) return;
        if (currentClipAmmo == gunData.maxClipSize || currentTotalAmmo == 0) return;
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);

        int need = gunData.maxClipSize - currentClipAmmo;
        int give = Mathf.Min(need, currentTotalAmmo);

        currentClipAmmo += give;
        currentTotalAmmo -= give;

        isReloading = false;
    }

    public int GetClip() => currentClipAmmo;
    public int GetTotal() => currentTotalAmmo;
    public int GetMaxClip() => gunData.maxClipSize;
}
