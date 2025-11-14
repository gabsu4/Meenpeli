using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{
   [SerializeField] MainMenuManager.MainMenuButtons _buttonType;
   public void ButtonClicked()
   {
    MainMenuManager._.MainMenuButtonClicked(_buttonType);
   }
}
