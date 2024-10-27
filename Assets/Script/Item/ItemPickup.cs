using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public UI_Interactable uiInteractable { get; private set; }
    public ItemData itemData;
    public int amount = 1;

    private void Start()
    {
        uiInteractable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
    }

    public void Interact()
    {
        AudioManager.PlayItemPickupSFX(transform.position);
        InventoryManager.SaveInventory(itemData, amount);
        Destroy(gameObject);
    }

    public InteractableData GetInteractableData()
    {
        return itemData;
    }

    private void OnDestroy()
    {
        //uiInteractable.Message(this);
    }

}
