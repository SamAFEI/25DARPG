using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    public int amount = 1;

    public void Interact()
    {
        InventoryManager.SaveInventory(itemData, amount);
        Destroy(gameObject);
    }

    public InteractableData GetInteractableData()
    {
        return itemData;
    }

}
