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
        Bowi equippedWeapon = null;

        if (playerEquipmentManager != null)
        {   
            playerEquipmentManager.EquipItem(itemToEquip);

            if (equippedWeapon == null)
            {
                Bowi temporaryBowi = playerEquipmentManager.gameObject.GetComponentInChildren<Bowi>(true); 

                if (temporaryBowi != null)
                {
                    temporaryBowi.ForceRegister(); 
                    equippedWeapon = Bowi.ActiveWeapon; 
                    Debug.Log("Bowi: Pakotettu rekisteröinti kutsuttu ja viite päivitetty.");
                }
                else
                {
                    Debug.LogError("Bowi rekisteröity."); 
                }
            }
        }
        if (ammoDisplay != null)
        {
            Debug.Log("HotBarController: Yritetään asettaa uusi ase AmmoDisplaylle.");
            ammoDisplay.SetCurrentWeapon(equippedWeapon);
        }
        else
        {
            Debug.LogError("HotBarController: AmmoDisplay on NULL. Linkitys epäonnistui Inspectorissa tai FindObjectOfType.");
        }

        if (itemToEquip != null && itemToEquip.IsConsumable())
        {
            itemToEquip.UseItem();
            Destroy(itemToEquip.gameObject);
            slots[selectedSlotIndex].currentItem = null;
        }
    }
}
