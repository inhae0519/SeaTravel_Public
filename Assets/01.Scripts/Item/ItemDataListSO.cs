using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemDataListInfo
{
    public ItemDataSo itemList;
    public int itemMinStack; 
    public int itemMaxStack;
}

[CreateAssetMenu(menuName = "SO/ItemList")]
public class ItemDataListSO : ScriptableObject
{
    public List<ItemDataListInfo> itemDataInfos;
}
