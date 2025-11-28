using UnityEngine;
using UnityEngine.InputSystem;

public class HotBarController : MonoBehaviour
{
    public EquipmentManager playerEquipmentManager;
    public Slot[] slots;

    public GameObject hotbarPanel;
    public int slotCount = 7;

    private int selectedSlotIndex = -1;

    private ItemDictionary itemDictionary;

    private UnityEngine.InputSystem.Key[] hotbarKeys;

    public void Awake()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();

        slots = new Slot[slotCount];
        hotbarKeys = new UnityEngine.InputSystem.Key[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            slots[i] = hotbarPanel.transform.GetChild(i).GetComponent<Slot>();

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

    void Start()
    {
        if (playerEquipmentManager == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player!= null)
            {
                playerEquipmentManager = player.GetComponent<EquipmentManager>();
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                SelectSlot(i);
                break;
            }
        }
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slotCount) return;

        if (selectedSlotIndex != -1 && selectedSlotIndex < slotCount)
        {
            slots[selectedSlotIndex].DeselectVisual();
        }

        selectedSlotIndex = index;
        slots[selectedSlotIndex].SelectVisual();

        ItemPickup itemToEquip = slots[selectedSlotIndex].currentItem;

        if (playerEquipmentManager != null)
        {
            playerEquipmentManager.EquipItem(itemToEquip);
        }

        if (itemToEquip != null && itemToEquip.IsConsumable())
        {
            itemToEquip.UseItem();
            Destroy(itemToEquip.gameObject);
            slots[selectedSlotIndex].currentItem = null;
        }
    }
}
