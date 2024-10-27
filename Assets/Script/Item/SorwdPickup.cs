using UnityEngine;

public class SorwdPickup : MonoBehaviour, IInteractable
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
        DoMessage();
        Destroy(gameObject);
    }

    public InteractableData GetInteractableData()
    {
        return itemData;
    }
    private void DoMessage()
    {
        uiInteractable.Message(this);
        Player player = GameManager.Instance.player.GetComponent<Player>();
        player.StartCoroutine(player.DemoSword3());
    }
}
