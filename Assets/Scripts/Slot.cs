using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemPickup currentItem;

    public GameObject itemGameObject;

    public GameObject selectionHighlight;

    void Start()
    {
        if (selectionHighlight != null)
        {
            selectionHighlight.SetActive(false);
        }
    }

    public void SelectVisual()
    {
        if (selectionHighlight != null)
        {
            selectionHighlight.SetActive(true);
        }
    }

    public void DeselectVisual()
    {
        if (selectionHighlight != null)
        {
            selectionHighlight.SetActive(false);
        }
    }
}
