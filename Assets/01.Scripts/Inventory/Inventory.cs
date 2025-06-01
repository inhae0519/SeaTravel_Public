using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoSingleton<Inventory>
{
    public MaterialStash materialStash;

    [Header("Inventory UI")] 
    [SerializeField] private Transform _materialStashParent;
    
    private void Awake()
    {
        materialStash = new MaterialStash(_materialStashParent);    
    }

    private void UpdateSlotUI()
    {
        materialStash.UpdateSlotUI();
    }

    public bool AddItem(ItemDataSo item, UnityAction usingFunc = null)
    {
        bool itemAdded = false;
        if (materialStash.CanAddItem(item))
        {
            if(usingFunc != null)
                materialStash.AddItem(item, usingFunc);
            else
                materialStash.AddItem(item);

            itemAdded = true;
        }
        if(itemAdded)
            UpdateSlotUI();

        return itemAdded;
    }

    public void RemoveItem(ItemDataSo item, int count = 1)
    {
        if (materialStash.HasItem(item))
        {
            materialStash.RemoveItem(item, count);
        }
        UpdateSlotUI(); 
    }
}
