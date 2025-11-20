using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController InventoryController;

    void Start()
    {
        InventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            ItemPickup pickup = collision.GetComponent<ItemPickup>();
            if(pickup != null && pickup.uiItemPrefab != null)
            {
                bool itemAdded = InventoryController.AddItem(pickup.uiItemPrefab);

                if (itemAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
