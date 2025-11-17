using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemData[] hotbarItems;
    public int hotbarSize = 5;

    public HotbarController hotbarController;

    private void Awake()
    {
        hotbarItems = new ItemData[hotbarSize];
    }

    public bool AddItem(ItemData itemToAdd)
    {
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null)
            {
                hotbarItems[i] = itemToAdd;
                if (hotbarController != null)
                {
                    hotbarController.UpdateSlot(i, itemToAdd);
                }
                return true;
            }
        }
        return false;
    }
    public ItemData GetItemInSlot(int index)
    {
        if (index >= 0 && index < hotbarItems.Length)
        {
            return hotbarItems[index];
        }
        return null;
    }
}