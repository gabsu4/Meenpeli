using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    public GameObject optionsCanvas;
    public GameObject inventoryCanvas;

    void Start()
    {
        optionsCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            optionsCanvas.SetActive(!optionsCanvas.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        }
    }
}