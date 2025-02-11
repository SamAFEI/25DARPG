using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveManager
{
    public static InventoryManager Instance { get; private set; }
    public List<Inventory> inventories = new List<Inventory>();
    public List<ItemData> items = new List<ItemData>();
    public List<ItemData> itemTypes = new List<ItemData>();

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

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
        itemTypes = Resources.LoadAll<ItemData>("Scriptable/Item").ToList();
    }

    public static void SaveInventory(ItemData itemData, int amount = 1)
    {
        if (amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                Instance.items.Add(itemData);
            }
        }
        else
        {
            for (int i = 0; i > amount; i--)
            {
                if (Instance.items.Contains(itemData))
                {
                    Instance.items.Remove(itemData);
                }
            }
        }
        GetInventories(); //刷新 不然Bag 會取到已消失的id 來取出Item
        if (Instance.onInventoryChangedCallback != null)
        {
            Instance.onInventoryChangedCallback();
        }
    }

    public static List<Inventory> GetInventories()
    {
        Instance.inventories = Instance.items.GroupBy(x => x).Select(x => new Inventory(x.Key, x.Count())).ToList();
        return Instance.inventories;
    }
    public static Inventory GetInventoy(string itemName)
    {
        return GetInventories().Where(x => x.item.name == itemName).FirstOrDefault();
    }

    public void LoadData(GameData _data)
    {
        if (_data.Inventories == null) return;
        inventories.AddRange(_data.Inventories);
        foreach (Inventory inventory in inventories)
        {
            ItemData item = itemTypes.Where(x => x.name == inventory.itemName).FirstOrDefault();
            if (item != null)
            {
                SaveInventory(item, inventory.amount);
            }
        }
        GetInventories();
    }

    public void SaveData(ref GameData _data)
    {
        _data.Inventories = GetInventories();
    }
}
