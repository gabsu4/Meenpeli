using System;
using UnityEngine;

public class Bowi : MonoBehaviour
{
    public float offset;
    public GameObject arrow;
    public Transform shotPoint;
    public float startTimeBtwShots;
    private float timeBtwShots;
    public int currentClip, maxClipSize = 10, currentAmmo, maxAmmoSize = 100;

    // Update is called once per frame
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
        int reloadAmount = maxClipSize - currentClip;
        reloadAmount = (currentAmmo - reloadAmount) >= 0 ? reloadAmount : currentAmmo;
        currentClip += reloadAmount;
        currentAmmo -= reloadAmount;
    }
    public void AddAmmo(int ammoAmount)
    {
        currentAmmo += ammoAmount;
        if (currentAmmo > maxAmmoSize)
        {
            currentAmmo = maxAmmoSize;
        }
    }
} 
