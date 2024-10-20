using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Shortcut : MonoBehaviour
{
    public static UI_Shortcut Instance { get; private set; }
    public List<UI_Slot> slots { get; private set; }
    public InventoryManager inventoryManager { get; private set; }
    public List<Inventory> inventories { get; private set; } = new List<Inventory>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
        
    }

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        inventoryManager.onInventoryChangedCallback += UpdateUI;
        InintSlots();
    }
    private void InintSlots()
    {
        slots = GetComponentsInChildren<UI_Slot>().ToList();
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].isShortcut = true;
        }

    }
    private void UpdateUI()
    {
        inventories = InventoryManager.GetInventories();

        //先找有在Shortcut的更新數量
        foreach (UI_Slot slot in slots)
        {
            if (slot.inventory == null) continue;
            Inventory inventory = inventories.Where(x => x.item.name == slot.itemName).FirstOrDefault();
            if (inventory != null)
            {
                slot.SetSlot(inventory);
                inventories.Remove(inventory);
            }
            else
            {
                slot.inventory.amount = 0;
                slot.SetSlot(slot.inventory);
            }
        }
        //沒有在Bag 要產生新的
        foreach (Inventory inventory in inventories)
        {
            foreach (UI_Slot slot in slots)
            {
                if (slot.inventory == null)
                {
                    slot.SetSlot(inventory);
                    break;
                }
            }
        }
    }

    public Inventory GetSlotItemData(int index)
    {
        return slots[index].inventory;
    }
}
