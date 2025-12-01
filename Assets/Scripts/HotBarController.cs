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

    public AmmoDisplay ammoDisplay;

    public void Awake()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();

        ammoDisplay = FindObjectOfType<AmmoDisplay>();

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
                return;
            }
        }

        if (selectedSlotIndex != -1 && Keyboard.current.eKey.wasPressedThisFrame)
            {
                UseSelectedConsumableItem();
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
    {
        Debug.Log("--- E-NÄPPÄINTÄ PAINETTU! ---"); // LISÄYS

        if (selectedSlotIndex != -1) 
        {
            UseSelectedConsumableItem();
        }
    }

        float scrollDelta = Mouse.current.scroll.y.ReadValue();

        if (scrollDelta != 0f)
        {
            int direction = 0;
            if (scrollDelta > 0)
            {
                direction = -1;
            }
            else if (scrollDelta < 0)
            {
                direction = 1;
            }

            int newIndex = selectedSlotIndex + direction;

            if (newIndex >= slotCount)
            {
                newIndex = 0;
            }
            else if (newIndex < 0)
            {
                newIndex = slotCount - 1;
            }
        
            SelectSlot(newIndex);
        }
    }

    public void UseSelectedConsumableItem()
    {
        if (selectedSlotIndex < 0 || selectedSlotIndex >= slotCount) return;

        Slot selectedSlot = slots[selectedSlotIndex];
        ItemPickup itemToUse = selectedSlot.currentItem;

        if (itemToUse != null && itemToUse.IsConsumable())
        {
            // 1. Kutsutaan UseItem() (esim. HealthPotion.cs)
            itemToUse.UseItem(); 

            // 2. Poistetaan esine slotti-inventaariosta
            Destroy(itemToUse.gameObject); 
            selectedSlot.currentItem = null; // Päivitetään slotti tyhjäksi
            
            // HUOM: Jos haluat heti käyttää seuraavan slotin, 
            // voit kutsua SelectSlot(selectedSlotIndex) tässä.
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
        IWeapon equippedWeapon = null;

        if (playerEquipmentManager != null)
        {
            equippedWeapon = playerEquipmentManager.EquipItem(itemToEquip);
        }
    
        if (ammoDisplay != null)
        {
            ammoDisplay.SetCurrentWeapon(equippedWeapon);
        
            if (equippedWeapon == null && itemToEquip != null && itemToEquip.isWeapon)
            {
                Debug.LogError("HotBarController: Asekomponenttia (Gun/Bowi) ei löytynyt aktiivisesta objektista. Tarkista komponentti/linkitykset!");
            }
        }
    }

}
