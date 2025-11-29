using UnityEngine;
using UnityEngine.UI;

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
                slot.itemGameObject = itemGo;
            }
        }
    }
}

    public bool AddItem(ItemPickup itemData) 
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItemGo = Instantiate(itemData.uiItemPrefab, slotTransform);
                newItemGo.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                Image itemImage = newItemGo.GetComponent<Image>();

                if (itemImage != null)
                {
                    itemImage.sprite = itemData.itemIcon;
                    itemImage.enabled = true;
                }

                slot.currentItem = itemData; 

                return true;
            }
        }
        return false;
    }
}