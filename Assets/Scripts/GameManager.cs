using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Vector2 nextSpawnPosition = Vector2.zero;

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
            DontDestroyOnLoad(gameObject); // <--- THIS is the key line
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }
}
