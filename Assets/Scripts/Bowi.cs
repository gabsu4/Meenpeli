using System;
using UnityEngine;

public class Bowi : MonoBehaviour
{
    public float offset;
    public GameObject arrow;
    public Transform shotPoint;
    public float startTimeBtwShots;
    private float timeBtwShots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = cursor - shotPoint.position;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion arrowRotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(arrow, shotPoint.position, arrowRotation);
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
} //https://www.youtube.com/watch?v=bY4Hr2x05p8 
