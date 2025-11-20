using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private AudioClip oviÄäni;
    public string nextSceneName = "Level02"; 

    private Keyinv playerInventory; 

    private bool playerIsAtDoor = false;

    private const string PlayerTag = "Player";
    public Vector2 destinationSpawnPoint;

    void Update()
    {   
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
            playerInventory = other.GetComponent<Keyinv>(); 
            Debug.Log("Press 'E' to use door.");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PlayerTag))
        {
            playerIsAtDoor = false;
            playerInventory = null; 
        }
    }
    private void LoadNextArea()
    {
        SoundManager.instance.PlaySound(oviÄäni);
        if (GameManager.Instance != null)
    {
        GameManager.Instance.nextSpawnPosition = destinationSpawnPoint;
    }

    SceneManager.LoadScene(nextSceneName);
    }
}
