using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager inventory = other.GetComponent<InventoryManager>();

            if (inventory != null)
            {
                if (inventory.AddItem(itemData))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}