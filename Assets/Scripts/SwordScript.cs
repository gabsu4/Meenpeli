using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SwordScript : MonoBehaviour
{
    public float meleeSpeed;
    public float damage;
    float timeUntilMelee;

    void Update()
    {
        if(timeUntilMelee <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                timeUntilMelee = meleeSpeed;
            }
        }
    }
}
//aika 8min https://www.youtube.com/watch?v=giJKCl-GVrU