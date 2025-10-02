using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChase : MonoBehaviour
{
    public GameObject testi_player;
    public float speed;

    private float distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, testi_player.transform.position);
        Vector2 direction = testi_player.transform.position - transform.position;

        if (distance < 6)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, testi_player.transform.position, speed * Time.deltaTime);
        }
    }
}
