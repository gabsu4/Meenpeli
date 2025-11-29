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

    public void SetItem(ItemPickup itemData, GameObject uiInstance)
    {
        if (itemGameObject != null)
        {
            Destroy(itemGameObject);
        }

        currentItem = itemData;

        itemGameObject = uiInstance; 
        
        if (itemGameObject != null)
        {
            Image visualImage = itemGameObject.GetComponent<Image>();
            
            if (visualImage != null)
            {
                visualImage.sprite = itemData.itemIcon;
                visualImage.enabled = true;
                visualImage.color = Color.white;
            }
        }
    }
}
