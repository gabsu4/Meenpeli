using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    // The name of the scene to load
    public string nextSceneName = "Level02"; 

    // A reference to the player's inventory script
    private Keyinv playerInventory; 

    // A flag to check if the player is currently standing at the door
    private bool playerIsAtDoor = false;

    // The tag of the object that acts as the player
    private const string PlayerTag = "Player";
    public Vector2 destinationSpawnPoint;

    void Update()
    {
        // Check if the player is at the door, has the key, AND presses 'E'
        if (playerIsAtDoor && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null && playerInventory.hasKey)
            {
                LoadNextArea();
            }
            else
            {
                Debug.Log("Door is locked. Find the key!");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PlayerTag))
        {
            playerIsAtDoor = true;
            // Get the player's inventory component when they enter
            playerInventory = other.GetComponent<Keyinv>(); 
            Debug.Log("Press 'E' to use door.");
        }
    }

    // Called when another collider exits the door's trigger area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PlayerTag))
        {
            playerIsAtDoor = false;
            playerInventory = null; // Clear reference when they leave
        }
    }
    private void LoadNextArea()
    {
        if (GameManager.Instance != null)
    {
        GameManager.Instance.nextSpawnPosition = destinationSpawnPoint;
    }

    // 2. Load the target scene (This destroys the current Player)
    SceneManager.LoadScene(nextSceneName);
    }
}
