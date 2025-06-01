using UnityEngine;

public class ItemInteract : Interactble {
    public ItemDataSo item;

    public override void Interact() { // æ∆¿Ã≈€ ∏‘±‚
        Inventory.Instance.AddItem(item, UsingFunc);
        //float stat = item.GetCurrentItemData();
        //StatManager.Instance.AddStat(stat, item.statType);
        Destroy(gameObject);
    }

    private void UsingFunc()
    {
    }
}
