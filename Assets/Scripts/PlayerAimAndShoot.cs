using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerAimAndShoot : MonoBehaviour
{
    
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    private GameObject bulletInst;
    private Vector2 worldPosition;
    private Vector2 direction;
    private float angle;

    private void Update()
    { 
        HandleGunRotation();
        HandleGunShooting();
    }
    
    private void HandleGunRotation()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosition - (Vector2)gun.transform.position).normalized;
        gun.transform.right = direction;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Vector3 localScale = gun.transform.localScale;
        
        if (angle > 90 || angle < -90)
        {
            if (localScale.y > 0)
            {
                localScale.y *= -1f;
            }
        }
        else
        {
            if (localScale.y < 0)
            {
                localScale.y *= -1f;
            }
        }
        gun.transform.localScale = localScale;
    }
    
    private void HandleGunShooting()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            bulletInst = Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
        }
    }
}