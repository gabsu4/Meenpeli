using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [SerializeField] private AudioClip oviÄäni;
    public GameObject pressEPrompt;
    public GameObject lockedPrompt;
    public string nextSceneName = "Level02"; 

    private Keyinv playerInventory; 

    private bool playerIsAtDoor = false;

    private const string PlayerTag = "Player";
    public Vector2 destinationSpawnPoint;
    private Coroutine hideLockedRoutine;

    void Start()
    {
        if (pressEPrompt != null)
        {
            pressEPrompt.SetActive(false);
        }
        if (lockedPrompt != null)
        {
            lockedPrompt.SetActive(false);
        }
    }

    void Update()
    {
        bool isLockedPromptVisible = (lockedPrompt != null && lockedPrompt.activeInHierarchy);
        if (pressEPrompt != null)
        {
            pressEPrompt.SetActive(playerIsAtDoor && !isLockedPromptVisible);
        }

        if (playerIsAtDoor && Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag(PlayerTag);
            Keyinv inventory = FindObjectOfType<Keyinv>();
        
        if (player != null)
        {
            if (playerInventory != null) {
        Debug.Log("Door check: playerInventory found. Key status: " + playerInventory.hasKey);
    } else {
        Debug.LogWarning("Door check: playerInventory is NULL. Did the player have Keyinv when entering?");
    }
            if (inventory != null && inventory.hasKey)
            {
                Debug.Log("Access Granted! Key found: " + inventory.hasKey);
                LoadNextArea();
            }
            else
            {
                Debug.Log("Access Denied! Key status: " + (inventory != null ? inventory.hasKey.ToString() : "NULL INVENTORY"));
                ShowLockedPrompt();
            }
        }
        }
    }

    private void ShowLockedPrompt()
    {
        if (lockedPrompt != null)
        {
            if (hideLockedRoutine != null)
            {
                StopCoroutine(hideLockedRoutine);
            }
            
            pressEPrompt.SetActive(false);
            lockedPrompt.SetActive(true);

            hideLockedRoutine = StartCoroutine(HideLockedPromptAfterDelay(2f)); 
        }
        
        Debug.Log("Door is locked. Find the key!"); 
    }

    IEnumerator HideLockedPromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (lockedPrompt != null)
        {
            lockedPrompt.SetActive(false);
        }
        
        if (playerIsAtDoor && pressEPrompt != null)
        {
            pressEPrompt.SetActive(true);
        }
        
        hideLockedRoutine = null;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag(PlayerTag))
        {
            playerIsAtDoor = true;
            Debug.Log("Press 'E' to use door.");
            if (pressEPrompt != null)
            {
                pressEPrompt.SetActive(playerIsAtDoor);
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PlayerTag))
        {
            playerIsAtDoor = false;
            if (hideLockedRoutine != null)
            {
                StopCoroutine(hideLockedRoutine);
            }
            if (lockedPrompt != null)
            {
                lockedPrompt.SetActive(false);
            }
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
