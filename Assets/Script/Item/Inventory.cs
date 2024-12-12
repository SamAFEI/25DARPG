[System.Serializable]
public class Inventory
{
    public ItemData item;
    public string itemName;
    public int amount;

    public Inventory(ItemData _itemData, int _amount)
    {
        this.item = _itemData;
        this.itemName = _itemData.name;
        this.amount = _amount;
    }
}
