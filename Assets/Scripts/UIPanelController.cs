using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    public GameObject mapCanvas;
    public GameObject inventoryCanvas;

    void Start()
    {
        mapCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            mapCanvas.SetActive(!mapCanvas.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        }
    }
}