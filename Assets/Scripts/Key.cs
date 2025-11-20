using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Keyinv inventory = other.GetComponent<Keyinv>();
            
            if (inventory != null)
            {
                inventory.CollectKey();
                Destroy(gameObject);    
            }
        }
    }
}
