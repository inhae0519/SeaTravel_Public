using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct StatTypeAndValue
{
    public PlayerStatEnum statType;
    public float value;
}

[CreateAssetMenu(menuName = "SO/Item")]
public class ItemDataSo : ScriptableObject
{
    public List<StatTypeAndValue> StatItems;
    // common
    public Sprite sprite;
    public UnityAction usingAction;
}