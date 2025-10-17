using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager _;
    [SerializeField] private bool _debugMode;
    public enum MainMenuButtons { play, options, quit };
    public enum OptionsButtons { back };
    [SerializeField] GameObject _MainMenuContainer;
    [SerializeField] GameObject _OptionsMenuContainer;
    [SerializeField] private string _sceneToLoadAfterClickingPlay;
    public void Awake()
    {
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("There are more than 1 MainMenuManager's in the scene");
        }
    }
    private void Start()
    {
        OpenMenu(_MainMenuContainer);
    }
    public void MainMenuButtonClicked(MainMenuButtons buttonClicked)
    {
        DebugMessage("Button Clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case MainMenuButtons.play:
            PlayClicked();
                break;
            case MainMenuButtons.options:
                OptionsClicked();
                break;
            case MainMenuButtons.quit:
                QuitGame();
                break;
            default:
                Debug.Log("Button clicked that wasn't implemented in MainMenuManager Method");
                break;
        }
    }
    public void OptionsClicked()
    {
        OpenMenu(_OptionsMenuContainer);    
    }
    public void ReturnToMainMenu()
    {
        OpenMenu(_MainMenuContainer);
    }
    public void OptionsButtonClicked(OptionsButtons buttonClicked)
    {
        switch (buttonClicked)
        {
            case OptionsButtons.back:
                ReturnToMainMenu();
                break;
        }
    }
    private void DebugMessage(string message)
    {
        if (_debugMode)
        {
            Debug.Log(message);
        }
    }
    public void PlayClicked()
    {
        SceneManager.LoadScene(_sceneToLoadAfterClickingPlay);
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
    public void OpenMenu(GameObject menuToOpen)
    {
        _MainMenuContainer.SetActive(menuToOpen == _MainMenuContainer);
        _OptionsMenuContainer.SetActive(menuToOpen == _OptionsMenuContainer);
    }
}
