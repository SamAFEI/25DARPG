public class Inventory
{
    public ItemData item { get; set; }
    public int amount { get; set; }

    public Inventory(ItemData _itemData, int _amount)
    {
        this.item = _itemData;
        this.amount = _amount;
    }
}
