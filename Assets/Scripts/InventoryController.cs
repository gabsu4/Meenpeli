using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    void Start()
    {
        for(int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if(i < itemPrefabs.Length)
            {
                GameObject itemGo = Instantiate(itemPrefabs[i], slot.transform);
                itemGo.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                ItemPickup itemComponent = itemGo.GetComponent<ItemPickup>();

                if (itemComponent != null)
                {
                    slot.currentItem = itemComponent;
                }
            }
        }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItemGo = Instantiate(itemPrefab, slotTransform);
                newItemGo.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                ItemPickup newItemComponent = newItemGo.GetComponent<ItemPickup>();

                if (newItemComponent != null)
                {
                    slot.currentItem = newItemComponent;
                    return true;
                }
            }
        }
        return false;
    }
}