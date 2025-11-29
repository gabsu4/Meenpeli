using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    private ItemPickup itemComponent;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        itemComponent = GetComponent<ItemPickup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
{
    Slot originalSlot = transform.parent.GetComponent<Slot>();
    if (originalSlot == null) return;

    originalParent = originalSlot.transform;
    originalSlot.currentItem = null;
    originalSlot.itemGameObject = null; 
    
    transform.SetParent(transform.root);
    canvasGroup.blocksRaycasts = false;
    canvasGroup.alpha = 0.6f;
}

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    
        Slot dropSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();
    
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if(dropSlot != null)
        {
            ItemPickup itemInDropSlotData = dropSlot.currentItem;
            GameObject itemInDropSlotVisual = dropSlot.itemGameObject;

            if (itemInDropSlotVisual != null)
            {
                itemInDropSlotVisual.transform.SetParent(originalParent);
                itemInDropSlotVisual.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            
                if (originalSlot != null)
                {
                    originalSlot.SetItem(itemInDropSlotData, itemInDropSlotVisual);
                }
            } 
            else if (originalSlot != null && itemInDropSlotVisual == null)
            {
            
            }

            transform.SetParent(dropSlot.transform);

            dropSlot.SetItem(itemComponent, gameObject);
        }
        else
        {
        
            transform.SetParent(originalParent);
        
            if (originalSlot != null)
            {
                originalSlot.SetItem(itemComponent, gameObject); 
            }
        }
        rectTransform.anchoredPosition = Vector2.zero;
    }
}