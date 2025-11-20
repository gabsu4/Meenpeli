using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        Slot originalSlot = originalParent.GetComponent<Slot>();
        if (originalSlot != null)
        {
            originalSlot.currentItem = null;
        }

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

        if(dropSlot != null)
        {
            GameObject itemInDropSlot = dropSlot.currentItem;

            if(itemInDropSlot != null)
            {
                itemInDropSlot.transform.SetParent(originalParent);
                originalParent.GetComponent<Slot>().currentItem = itemInDropSlot;
                itemInDropSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            transform.SetParent(originalParent);
            originalParent.GetComponent<Slot>().currentItem = gameObject;
        }
        
        rectTransform.anchoredPosition = Vector2.zero;
    }
}