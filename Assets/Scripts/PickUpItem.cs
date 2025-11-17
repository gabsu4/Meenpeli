using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger with: " + other.name);

        // Etsitään InventoryManager myös parent objekteista (tärkein muutos!)
        InventoryManager inventory = other.GetComponentInParent<InventoryManager>();

        if (inventory != null)
        {
            Debug.Log("InventoryManager FOUND!");

            if (inventory.AddItem(itemData))
            {
                Debug.Log("Item added to inventory!");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Hotbar full!");
            }
        }
        else
        {
            Debug.LogWarning("InventoryManager NOT FOUND on object: " + other.name);
        }
    }
}
