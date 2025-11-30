using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject uiItemPrefab;
    
    public int ID;
    public string Name;
    public Sprite itemIcon;

    public bool isConsumableItem = false;

    public bool isWeapon = false;

    public virtual void UseItem()
    {
        Debug.Log("Using item" + Name);
    }

    public bool IsConsumable()
    {
        return isConsumableItem;
    }

    public void Pickup()
    {
        Sprite icon = this.itemIcon;
        
        if(ItemPickUpUIController.Instance != null)
        {
            ItemPickUpUIController.Instance.ShowItemPickup(Name, icon);
        }
    }
}