using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    private ItemData currentlySelectedItem;
    public PlayerAimAndShoot gunController;

    public void SetSelectedItem(ItemData item)
    {
        currentlySelectedItem = item;

        if (gunController != null)
            gunController.SetGun(null); // oletuksena pois päältä

        if (item == null) return;

        if (item.itemType == ItemType.Gun)
        {
            GunItemData gun = item as GunItemData;
            gunController.SetGun(gun);
        }
    }
}
