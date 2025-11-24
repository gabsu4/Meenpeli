using UnityEngine;
using UnityEngine.InputSystem;

public class HotBarController : MonoBehaviour
{
    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount = 10;

    private ItemDictionary itemDictionary;

    private UnityEngine.InputSystem.Key[] hotbarKeys;

    public void Awake()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();

        hotbarKeys = new UnityEngine.InputSystem.Key[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            if (i < 9)
            {
                hotbarKeys[i] = (UnityEngine.InputSystem.Key) ((int)UnityEngine.InputSystem.Key.Digit1 + i);
            }
            else
            {
                hotbarKeys[i] = UnityEngine.InputSystem.Key.Digit0;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot(int index)
    {
        Slot slot = hotbarPanel.transform.GetChild(index).GetComponent<Slot>();
        if(slot.currentItem != null)
        {
            ItemPickup item = slot.currentItem.GetComponent<ItemPickup>();
            item.UseItem();
        }
    }
}
