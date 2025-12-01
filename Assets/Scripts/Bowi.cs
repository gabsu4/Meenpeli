using System;
using UnityEngine;

public class Bowi : MonoBehaviour, IWeapon
{
    public float offset;
    public GameObject arrow;
    public Transform shotPoint;
    public float startTimeBtwShots;
    private float timeBtwShots;
    
    public int currentClip { get; private set; } = 10; 
    public int maxClipSize = 10;
    public int currentAmmo { get; private set; } = 100;
    public int maxAmmoSize = 100;

    public event Action<int, int> OnAmmoChanged;

void OnDisable()
{
    OnAmmoChanged = null; 
}

    public void ForceRegister()
    {
        if (currentClip == 0 && currentAmmo == 0)
    {
        currentClip = maxClipSize;
        currentAmmo = maxAmmoSize;
    }
    InvokeAmmoChange();
    }

    void Start()
    {

    }

    void Update()
    {
        Vector3 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = cursor - shotPoint.position;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion arrowRotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (timeBtwShots <= 0)
        {
            if (currentClip > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(arrow, shotPoint.position, arrowRotation);
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
    public void Reload()
    {
        int ammoNeeded = maxClipSize - currentClip;
        int originalCurrentAmmo = currentAmmo;
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
        Debug.Log($"Bowi: Lähettää ammuspäivityksen: Clip={currentClip}, Reserve={currentAmmo}");
        OnAmmoChanged?.Invoke(currentClip, currentAmmo);
    }
} 
