using UnityEngine;

public class Keyinv : MonoBehaviour
{
    public bool hasKey = false; // Starts false
    
    // Call this method when the player picks up the key item
    public void CollectKey()
    {
        hasKey = true;
        Debug.Log("Key Collected!");
    }
}
