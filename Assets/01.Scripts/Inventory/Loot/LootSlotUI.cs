using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LootSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _itemText;

    public InventoryItem item;
    public void SetLootSlot(ItemDataSo itemDataSo, int stack)
    {
        item = new InventoryItem(itemDataSo, stack);
        if(!CheckSlot())
            return;
        _itemImage.sprite = item.data.sprite;
        _itemImage.gameObject.SetActive(true);
        _itemText.text = item.stackSize.ToString();
    }

    private bool CheckSlot()
    {
        if (item.stackSize < 1)
        {
            CleanUpSlot();
            return false;
        }
        else
        {
            _itemText.text = item.stackSize.ToString();
            return true;
        }
    }
    
    public void CleanUpSlot()
    {
        item = null;
        _itemImage.gameObject.SetActive(false);
        _itemText.text = String.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;
        
        item.RemoveStack();
        LootInventory.Instance.RemoveLootSlot(item.data);
        CheckSlot();
    }
}
