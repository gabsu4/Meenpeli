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

        if(dropSlot != null)
        {
            ItemPickup itemInDropSlotData = dropSlot.currentItem;
            GameObject itemInDropSlot = dropSlot.itemGameObject;

            if(itemInDropSlot != null)
            {
                itemInDropSlot.transform.SetParent(originalParent);
                itemInDropSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                Slot originalSlot = originalParent.GetComponent<Slot>();
                if (originalSlot != null)
                {
                    originalSlot.currentItem = itemInDropSlotData;
                    originalSlot.itemGameObject = itemInDropSlot;
                }
            }

            transform.SetParent(dropSlot.transform);

            dropSlot.currentItem = itemComponent;
            dropSlot.itemGameObject = gameObject;
        }
        else
        {
            Slot originalSlot = originalParent.GetComponent<Slot>();

            transform.SetParent(originalParent);

            if (originalSlot != null)
            {
                originalSlot.currentItem = itemComponent;
                originalSlot.itemGameObject = gameObject;
            }
        }
        
        rectTransform.anchoredPosition = Vector2.zero;
    }
}