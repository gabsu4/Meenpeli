using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject uiItemPrefab;
    
    public int ID;
    public string Name;
    public Sprite itemIcon;

    public void Pickup()
    {
        Sprite icon = this.itemIcon;
        
        if(ItemPickUpUIController.Instance != null)
        {
            ItemPickUpUIController.Instance.ShowItemPickup(Name, icon);
        }
    }
}