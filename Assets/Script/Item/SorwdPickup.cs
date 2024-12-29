using UnityEngine;

public class SorwdPickup : MonoBehaviour, IInteractable
{
    public UI_Interactable uiInteractable { get; private set; }
    public ChartIndex chartIndex;
    public ItemData itemData;
    public int amount = 1;

    private void Start()
    {
        uiInteractable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
    }
    public void Interact()
    {
        if (GameManager.Instance.isBattleing)
        {
            AudioManager.PlayCancelSFX(transform.position);
            return;
        }
        AudioManager.PlayItemPickupSFX(transform.position);
        InventoryManager.SaveInventory(itemData, amount);
        FlowManager.ExecuteChart(chartIndex);
        if (chartIndex == ChartIndex.LearnEarthshatter)
        {
            FlowManager.SetBooleanVariable(ChartIndex.LearnEarthshatter.ToString(), true);
        }
        Destroy(gameObject);
    }

    public InteractableData GetInteractableData()
    {
        return itemData;
    }
}
