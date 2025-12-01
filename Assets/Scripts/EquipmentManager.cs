using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public GameObject daggerObject;
    public GameObject swordObject;
    public GameObject bowObject;
    public GameObject gunObject;

    private GameObject currentWeaponObject = null;

    void Start()
    {
        DisableAllWeapons();
    }

    private void DisableAllWeapons()
    {
        if (daggerObject != null) daggerObject.SetActive(false);
        if (swordObject != null) swordObject.SetActive(false);
        if (bowObject != null) bowObject.SetActive(false);
        if (gunObject != null) gunObject.SetActive(false);

        currentWeaponObject = null;
    }

    public IWeapon EquipItem(ItemPickup itemToEquip)
    {
        DisableAllWeapons();

        if (itemToEquip == null)
        {
            return null;
        }

        GameObject weaponToActivate = null;

        if (itemToEquip.Name == "Dagger")
        {
            weaponToActivate = daggerObject;
        }
        else if (itemToEquip.Name == "Sword")
        {
            weaponToActivate = swordObject;
        }
        else if (itemToEquip.Name == "Bow")
        {
            weaponToActivate = bowObject;
        }
        else if (itemToEquip.Name == "Gun")
        {
            weaponToActivate = gunObject;
        }
        else if (itemToEquip.Name == "Potion")
        {
            UseConsumable(itemToEquip);
            return null;
        }
        else
        {
            Debug.Log($"Item {itemToEquip.Name} is not recognized as a weapon.");
            return null;
        }

        if (weaponToActivate != null)
        {
            weaponToActivate.SetActive(true);
            currentWeaponObject = weaponToActivate;

            IWeapon equippedWeapon = weaponToActivate.GetComponent<IWeapon>(); 
        
            return equippedWeapon; // <-- PALAUTA VIITE SUORAAN!
        }

        return null;
    }

    private void UseConsumable(ItemPickup potion)
    {
        Debug.Log("Player used a Healing Potion!");
    }
}
