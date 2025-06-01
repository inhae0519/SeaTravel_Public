using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _itemText;

    public InventoryItem item;
    
    public void UpdateSlot(InventoryItem inventoryItem)
    {
        item = inventoryItem;
        _itemImage.sprite = item.data.sprite;
        _itemImage.gameObject.SetActive(true);

        if (item != null)
        {
            if (item.stackSize > 1)
            {
                _itemText.text = item.stackSize.ToString();
            }
            else
            {
                _itemText.text = String.Empty;
            }
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
        if (eventData.button == PointerEventData.InputButton.Right && item.data != null)
        {   
            UIManager.Instance.OnUsingPanel(item.data, eventData.position);
        }
    }
}
