using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10;

    public AmmoType type = AmmoType.Arrow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        
        if (type == AmmoType.Arrow)
        {
            Bowi bowi = collision.gameObject.GetComponentInChildren<Bowi>();
            if (bowi != null)
            {
                bowi.AddAmmo(ammoAmount);
                Destroy(gameObject);
                return;
            }
        }
        else if (type == AmmoType.Bullet)
        {
            Gun gun = collision.gameObject.GetComponentInChildren<Gun>();
            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
                Destroy(gameObject);
                return;
            }
        }
    }
}
// https://www.youtube.com/watch?v=cjNMQkODh1M