using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUseItem : MonoBehaviour
{
    private ItemData currentlySelectedItem;

    public PlayerAimAndShoot playerGunController;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            UseSelectedItem();
        }
    }
    public void SetSelectedItem(ItemData item)
    {
        currentlySelectedItem = item;

        if (playerGunController != null)
        {
            playerGunController.enabled = false; 
        }

        if (currentlySelectedItem != null)
        {
            if (currentlySelectedItem.itemType == ItemType.Gun && playerGunController != null)
            {
                playerGunController.enabled = true; 
            }
        }
    }

    private void UseSelectedItem()
    {
        if (currentlySelectedItem == null)
        {
            return;
        }
    }
}