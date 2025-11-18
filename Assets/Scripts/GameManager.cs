using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
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
    public void EndGame()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
        }
        
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        isGameOver = false;

        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(false);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}