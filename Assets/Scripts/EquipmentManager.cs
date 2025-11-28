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

    public void EquipItem(ItemPickup itemToEquip)
    {
        if (itemToEquip == null)
        {
            DisableAllWeapons();
            return;
        }

        DisableAllWeapons();

        if (itemToEquip.Name == "Dagger")
        {
            daggerObject.SetActive(true);
            currentWeaponObject = daggerObject;
        }
        else if (itemToEquip.Name == "Sword")
        {
            swordObject.SetActive(true);
            currentWeaponObject = swordObject;
        }
        else if (itemToEquip.Name == "Bow")
        {
            bowObject.SetActive(true);
            currentWeaponObject = bowObject;
        }
        else if (itemToEquip.Name == "Gun")
        {
            gunObject.SetActive(true);
            currentWeaponObject = gunObject;
        }
        else if (itemToEquip.Name == "Potion")
        {
            UseConsumable(itemToEquip);
        }
        else
        {
            Debug.Log($"Item {itemToEquip.Name} is not recognized as a weapon.");
        }
    }

    private void UseConsumable(ItemPickup potion)
    {
        Debug.Log("Player used a Healing Potion!");
    }
}
