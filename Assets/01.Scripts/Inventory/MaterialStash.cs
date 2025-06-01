using UnityEngine;
using UnityEngine.Events;

public class MaterialStash : Stash
{
    public MaterialStash(Transform parent) : base(parent)
    {
    }

    public override void AddItem(ItemDataSo item, UnityAction usingFunc = null)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            item.usingAction = usingFunc;
            InventoryItem newItem = new InventoryItem(item);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }

    public override void RemoveItem(ItemDataSo itemData, int count)
    {
        if (stashDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            if (value.stackSize <= count)
            {
                stash.Remove(value);
                stashDictionary.Remove(itemData);
            }
            else
            {
                value.RemoveStack(count);
            }
        }
    }

    public override bool CanAddItem(ItemDataSo itemData)
    {
        if (stashDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            return true;
        }

        if (stash.Count >= _itemSlots.Length)
        {
            Debug.Log("No more space");
            return false;
        }

        return true;
    }
}
