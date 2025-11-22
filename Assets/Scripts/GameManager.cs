using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private UIManager uiManager;
    public GameObject deathScreenUI;
    public Vector2 nextSpawnPosition = Vector2.zero;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>(); 
    
        if (uiManager == null)
        {
            Debug.LogError("GameManager couldn't find a UIManager in the scene!");
        }
    }
    public void EndGame()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        
        if (uiManager != null)
        {
            uiManager.ToggleDeathPanel();
        }
        
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        isGameOver = false;

        if (uiManager != null)
        {
            if (uiManager.deathPanel.activeSelf)
            {
                uiManager.ToggleDeathPanel();
            }
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RegisterDeathScreenUI(GameObject uiObject)
    {
        deathScreenUI = uiObject;
        deathScreenUI.SetActive(false);
    }
}