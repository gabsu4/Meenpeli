using System;
using UnityEngine;

public class Bowi : MonoBehaviour
{
    public float offset;
    public GameObject arrow;
    public Transform shotPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(arrow, shotPoint.position, transform.rotation);
        }
    }
} //https://www.youtube.com/watch?v=bY4Hr2x05p8 min 3.45 ja kato sevvalnidabal kommentti
