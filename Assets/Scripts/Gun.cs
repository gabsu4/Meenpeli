using System;
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour, IWeapon
{
    public float offset;
    public GameObject bullet;
    public Transform spawnPoint;
    public float startTimeBtwShots;
    private float timeBtwShots;

    public event Action<int, int> OnAmmoChanged;
    
    public int currentClip { get; private set; } = 10;
    public int maxClipSize = 10;
    public int currentAmmo { get; private set; } = 100;
    public int maxAmmoSize = 100;

    void OnDisable()
    {
        OnAmmoChanged = null; 
        Debug.Log("Gun: OnDisable kutsuttu. OnAmmoChanged nollattu.");
    }
    
    public void ForceRegister()
    {
        StartCoroutine(DelayForceRegister());
    }

    private IEnumerator DelayForceRegister()
    {
        yield return null; 

        if (currentClip == 0 && currentAmmo == 0)
        {
            currentClip = maxClipSize;
            currentAmmo = maxAmmoSize;
        }
        InvokeAmmoChange();
        Debug.Log("Gun: ForceRegister (viivästetty) kutsuttu. Ammuspäivitys lähetetty.");
    }
    
    void Start()
    {

    }

    void Update()
    {
        Vector3 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = cursor - spawnPoint.position;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion bulletRotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (timeBtwShots <= 0)
        {
            if (currentClip > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(bullet, spawnPoint.position, bulletRotation);
                    timeBtwShots = startTimeBtwShots;
                    currentClip--;

                    InvokeAmmoChange();
                }
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
    
    // --- AMMUSLIIKETOIMINTA ---

    public void Reload()
    {
        int ammoNeeded = maxClipSize - currentClip;
        int ammoToTransfer = Mathf.Min(ammoNeeded, currentAmmo);

        currentClip += ammoToTransfer;
        currentAmmo -= ammoToTransfer;

        if (ammoToTransfer > 0)
        {
            InvokeAmmoChange();
        }
    }
    
    public void AddAmmo(int ammoAmount)
    {
        int originalCurrentAmmo = currentAmmo;
        currentAmmo += ammoAmount;
        if (currentAmmo > maxAmmoSize)
        {
            currentAmmo = maxAmmoSize;
        }

        if (currentAmmo != originalCurrentAmmo)
        {
            InvokeAmmoChange();
        }
    }

    private void InvokeAmmoChange()
    {
        Debug.Log($"Gun: Lähettää ammuspäivityksen: Clip={currentClip}, Reserve={currentAmmo}");
        OnAmmoChanged?.Invoke(currentClip, currentAmmo);
    }
}