using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the object entering the trigger is the Player (using its tag).
        if (other.CompareTag("Player"))
        {
            // 2. Try to get the player's inventory script (KeyInventory).
            Keyinv inventory = other.GetComponent<Keyinv>();
            
            // 3. If the Player has the inventory script, collect the key.
            if (inventory != null)
            {
                inventory.CollectKey(); // This sets hasKey = true in the player's script.
                
                // 4. Destroy the key object, removing it from the game world.
                Destroy(gameObject);    
            }
        }
    }
}
