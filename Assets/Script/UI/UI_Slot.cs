using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public string itemName { get; private set; }
    public Image icon { get; private set; }
    public TextMeshProUGUI amount { get; private set; }
    public RectTransform iconRectTransform { get; private set; }
    public CanvasGroup canvasGroup { get; private set; }
    public Inventory inventory;
    public ItemData item;
    public bool isShortcut;
    public GameObject dragImage;

    private void Awake()
    {
        icon = transform.Find("UI_Icon").GetComponent<Image>();
        amount = transform.Find("UI_Amount").GetComponent<TextMeshProUGUI>();
        iconRectTransform = icon.GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        ChangeItemData(item);
    }


    public void SetSlot(Inventory _inventory)
    {
        inventory = _inventory;
        if (_inventory == null)
        {
            RemoveSlot();
            return;
        }
        ChangeItemData(inventory.item);
        amount.text = inventory.amount.ToString();
    }

    public void RemoveSlot()
    {
        inventory = null;
        ChangeItemData(null);
    }

    public void ChangeItemData(ItemData _item)
    {
        HelperUtilities.SetColorAlpha(icon,0);
        HelperUtilities.SetColorAlpha(amount, 0);
        item = _item;
        if (_item != null)
        {
            itemName = _item.name;
            icon.sprite = _item.icon;
            HelperUtilities.SetColorAlpha(icon, 1);
            HelperUtilities.SetColorAlpha(amount, 1);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inventory == null || isShortcut) { return; }
        canvasGroup.blocksRaycasts = false;
        dragImage = Instantiate(new GameObject(), transform.parent);
        dragImage.AddComponent<RectTransform>();
        dragImage.AddComponent<Image>().sprite = icon.sprite;
        dragImage.GetComponent<Image>().raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        iconRectTransform.position = GetComponent<RectTransform>().position;
        canvasGroup.blocksRaycasts = true;
        Destroy(dragImage);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inventory == null || isShortcut) { return; }
        iconRectTransform.position = eventData.position;
        dragImage.GetComponent<RectTransform>().position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        UI_Slot dropSlot = eventData.pointerDrag.GetComponent<UI_Slot>();
        Inventory dropInventory = dropSlot.inventory;
        if (dropSlot.isShortcut || dropInventory == null ||
            (isShortcut && !dropInventory.item.canShortcut)) 
        { return; }
        if (!isShortcut)
        {
            dropSlot.SetSlot(inventory);
        }
        SetSlot(dropInventory);
    }
}
