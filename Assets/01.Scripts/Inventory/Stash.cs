using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Stash
{
    public List<InventoryItem> stash;
    public Dictionary<ItemDataSo, InventoryItem> stashDictionary;
    protected Transform _slotParent;
    protected ItemSlotUI[] _itemSlots;

    public Stash(Transform parent)
    {
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemDataSo, InventoryItem>();

        _itemSlots = parent.GetComponentsInChildren<ItemSlotUI>();
    }

    public virtual void UpdateSlotUI()
    {
        for (int i = 0; i < _itemSlots.Length; ++i)
        {
            _itemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stash.Count; ++i)
        {
            _itemSlots[i].UpdateSlot(stash[i]);
        }
    }

    public virtual bool HasItem(ItemDataSo itemData)
    {
        return stashDictionary.ContainsKey(itemData);
    }
    public abstract void AddItem(ItemDataSo item, UnityAction usingFunc = null);
    public abstract void RemoveItem(ItemDataSo itemData, int count);
    public abstract bool CanAddItem(ItemDataSo itemData);
}
