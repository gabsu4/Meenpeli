using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditController : MonoBehaviour
{
    [SerializeField]
    private float creditsDuration = 16.5f;
    [SerializeField]
    private string mainMenuSceneName = "Main Menu"; 

    private float timer;
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= creditsDuration)
        {
            LoadMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
