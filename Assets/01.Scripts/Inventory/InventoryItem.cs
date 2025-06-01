using System;

[Serializable]
public class InventoryItem
{
    public ItemDataSo data;
    public int stackSize;
    
    public InventoryItem(ItemDataSo newItemData, int count = 1)
    {
        data = newItemData;
        AddStack(count);
    }

    public void AddStack(int count = 1)
    {
        stackSize += count;
    }

    public void RemoveStack(int count = 1)
    {
        stackSize -= count;
    }
}