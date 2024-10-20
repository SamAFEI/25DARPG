using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public static List<ItemData> items = new List<ItemData>();
    public static List<Inventory> inventories = new List<Inventory>();

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

    public static void SaveInventory(ItemData itemData, int amount = 1)
    {
        if (amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                items.Add(itemData);
            }
        }
        else
        {
            for (int i = 0; i > amount; i--)
            {
                if (items.Contains(itemData))
                {
                    items.Remove(itemData); 
                }
            }
        }
        if (Instance.onInventoryChangedCallback != null)
        {
            Instance.onInventoryChangedCallback();
        }
    }

    public static List<Inventory> GetInventories()
    {
        inventories = items.GroupBy(x => x).Select(x => new Inventory(x.Key, x.Count())).ToList();
        return inventories;
    }
    public static Inventory GetInventoy(string itemName)
    {
        return GetInventories().Where(x => x.item.name == itemName).FirstOrDefault();
    }
}
