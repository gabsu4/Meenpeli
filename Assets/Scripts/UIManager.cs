using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject deathPanel;
    
    public void ToggleDeathPanel()
    {
        deathPanel.SetActive(!deathPanel.activeSelf);
    }
}
