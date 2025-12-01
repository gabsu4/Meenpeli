using UnityEngine;

public class HealthPotion : ItemPickup
{
    [SerializeField] private int healAmount = 20; 

    public override void UseItem()
    {
        base.UseItem(); 

        // 1. Etsitään objekti "Player"-tagin avulla (LUOTETTAVA)
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); 
        
        if (playerObject != null)
        {
            // 2. Haetaan Health-komponentti LÖYDETTYLTÄ pelaajaobjektilta.
            Health playerHealth = playerObject.GetComponentInChildren<Health>();
        
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Debug.Log("Player healed for " + healAmount + " health!");
            }
            else
            {
                 // Virhe, jos Tag on oikein, mutta komponentti puuttuu.
                 Debug.LogError("HealthPotion: Player-objektilta puuttuu Health-komponentti, vaikka Tag löytyi!");
            }
        }
        else
        {
            // Virhe, jos Tagia "Player" ei löydy.
            Debug.LogError("HealthPotion: Player-objektia (Tag: Player) ei löytynyt! Oletko asettanut Tagin?");
        }
    }
}