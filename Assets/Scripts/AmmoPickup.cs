using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bowi bowi = collision.gameObject.GetComponentInChildren<Bowi>();
        if (bowi)
        {
            bowi.AddAmmo(bowi.maxAmmoSize);
            Destroy(gameObject);
        }
    }
}
// https://www.youtube.com/watch?v=cjNMQkODh1M