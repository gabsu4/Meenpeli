using UnityEngine;

public class Keyinv : MonoBehaviour
{
    [SerializeField] private AudioClip pickup;
    public bool hasKey = false;
    
    public void CollectKey()
    {
        SoundManager.instance.PlaySound(pickup);
        hasKey = true;
        Debug.Log("Key Collected!");
    }
}
