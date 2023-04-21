using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEnum
{
    Key,
    Potion
}

[CreateAssetMenu(menuName = "Item Data", fileName = "NewItemData", order = 2)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    public ItemEnum itemType;
    [SerializeField]
    public GameObject itemPrefab;
    [SerializeField]
    public float potionHealValue;


    public ItemEnum ItemType { get; }
    public GameObject ItemPrefab { get; }
    public float PotionHealValue { get; set; }
}
