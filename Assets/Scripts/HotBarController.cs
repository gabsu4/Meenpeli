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

        // Laske uusi indeksi
        int newIndex = selectedSlotIndex + direction;

        // Käsittele kääriytyminen (wrapping)
        if (newIndex >= slotCount)
        {
            newIndex = 0; // Siirry viimeisestä slottista ensimmäiseen
        }
        else if (newIndex < 0)
        {
            newIndex = slotCount - 1; // Siirry ensimmäisestä slottista viimeiseen
        }
        
        // Varmista, että uusi indeksi on validi ja valitaan
        SelectSlot(newIndex);
    }
    }

    // HotBarController.cs - UUSI SelectSlot-funktio (EI COROUTINEA)

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
        IWeapon equippedWeapon = null; // Määrittele viite tähän

        if (playerEquipmentManager != null)
        {   
        // Vastaanota IWeapon-viite suoraan EquipItem-kutsusta!
            equippedWeapon = playerEquipmentManager.EquipItem(itemToEquip);
        }
    
    // UUSI KOODI: Käsittele ammusnäyttö heti
        if (ammoDisplay != null)
        {
            ammoDisplay.SetCurrentWeapon(equippedWeapon);
        
            if (equippedWeapon == null && itemToEquip != null && itemToEquip.isWeapon)
            {
             // Jos on ase, mutta viite puuttuu, vika on komponentissa.
                Debug.LogError("HotBarController: Asekomponenttia (Gun/Bowi) ei löytynyt aktiivisesta objektista. Tarkista komponentti/linkitykset!");
            }
        }

    // Vanha Consumable-logiikka pysyy ennallaan
        if (itemToEquip != null && itemToEquip.IsConsumable())
        {
            itemToEquip.UseItem();
            Destroy(itemToEquip.gameObject);
            slots[selectedSlotIndex].currentItem = null;
        }
    }

}
