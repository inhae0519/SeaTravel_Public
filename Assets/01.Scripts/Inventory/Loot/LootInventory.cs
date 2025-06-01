using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class LootInventory : MonoSingleton<LootInventory>
{
    [SerializeField] private AudioClip eat;
    [SerializeField] private Transform lootSlotStash;
    private Dictionary<ItemDataSo, int> currentSpotDic;
    
    public void SetLootStash(Dictionary<ItemDataSo, int> itemDictionary, List<int> slotIndex)
    {
        ClearLootSlot();
        currentSpotDic = itemDictionary;
        int index = 0;
        foreach (var item in itemDictionary)
        {
            lootSlotStash.GetChild(slotIndex[index]).GetComponent<LootSlotUI>().SetLootSlot(item.Key, item.Value);
            index++;
        }
    }

    public void ClearLootSlot()
    {
        foreach (var lootSlot in lootSlotStash.GetComponentsInChildren<LootSlotUI>())
        {
            lootSlot.CleanUpSlot();
        }
    }

    public void RemoveLootSlot(ItemDataSo itemDataSo, int stack = 1)
    {
        currentSpotDic[itemDataSo] -= stack;
        Inventory.Instance.AddItem(itemDataSo, ()=>
        {
            foreach (StatTypeAndValue statTypeAndValue in itemDataSo.StatItems)
            {
                SoundManager.Instance.PlayEffect(eat);
                PlayerManager.Instance.AddPlayerStat(statTypeAndValue);
            }
        });
    }
}
