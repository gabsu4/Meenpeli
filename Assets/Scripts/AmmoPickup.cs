using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmmoPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        
        Bowi bowi = collision.gameObject.GetComponentInChildren<Bowi>();
        if (bowi)
        {
            bowi.AddAmmo(bowi.maxAmmoSize);
            Destroy(gameObject);
        }
    }
}
// https://www.youtube.com/watch?v=cjNMQkODh1M