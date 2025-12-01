using UnityEngine;

public class HealthPotion : ItemPickup
{
    [SerializeField] private int healAmount = 20; 

    public override void UseItem()
    {
        base.UseItem(); 

        Health playerHealth = FindObjectOfType<Health>();
        
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
            Debug.Log("Player healed for " + healAmount + " health! (E-key works!)");

        }
        else
        {
            Debug.LogError("HealthPotion: Health.cs-skriptiä ei löydy pelistä. Onko se kiinnitetty ja aktiivinen?");
        }
    }
}