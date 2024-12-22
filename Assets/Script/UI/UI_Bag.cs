using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Bag : MonoBehaviour
{
    public static UI_Bag Instance { get; private set; }
    public InventoryManager inventoryManager { get; private set; }
    public List<Inventory> inventories { get; private set; } = new List<Inventory>();

    public GameObject UI_Area;
    public int slotsWidth = 4, slotsHeight = 2;
    public UI_Slot[,] slots;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitUI();
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
    }

    private void InitUI()
    {
        UI_Area.SetActive(true);
        slots = new UI_Slot[slotsHeight, slotsWidth];
        List<UI_Slot> list = GetComponentsInChildren<UI_Slot>().ToList();
        //foreach (UI_Slot slot in list)
        //{
        //    Debug.Log(slot.gameObject.name);
        //}
        for (int i = 0; i < slotsHeight; i++)
        {
            for (int j = 0; j < slotsWidth; j++)
            {
                slots[i, j] = list[0];
                list.RemoveAt(0);
            }
        }
        UI_Area.SetActive(false);
    }

    private void UpdateUI()
    {
        if (!UI_Area.activeSelf) { return; }
        inventories = new List<Inventory>();
        inventories.AddRange(InventoryManager.GetInventories());

        //先找有在Bag的更新數量
        for (int i = 0; i < slotsHeight; i++)
        {
            for (int j = 0; j < slotsWidth; j++)
            {
                UI_Slot slot = slots[i, j];
                if (slot.inventory == null) continue;
                Inventory inventory = inventories.Where(x => x.item.name == slot.itemName).FirstOrDefault();
                if (inventory != null)
                {
                    slot.SetSlot(inventory);
                    inventories.Remove(inventory);
                }
                else
                {
                    slot.RemoveSlot();
                }
            }
        }
        //沒有在Bag 要產生新的
        foreach (Inventory inventory in inventories)
        {
            bool hasCreate = false;
            for (int i = 0; i < slotsHeight; i++)
            {
                for (int j = 0; j < slotsWidth; j++)
                {
                    UI_Slot slot = slots[i, j];
                    if (slot.inventory == null)
                    {
                        slot.SetSlot(inventory);
                        hasCreate = true;
                        break;
                    }
                }
                if (hasCreate) break;
            }
        }
    }

    public void OpenUI_Area()
    {
        if (!UI_Area.activeSelf)
        {
            UI_Area.SetActive(true);
            UpdateUI();
            return;
        }
        UI_Area.SetActive(false);
    }
}

