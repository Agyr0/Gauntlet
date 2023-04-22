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



    public ItemEnum ItemType
    { get { return itemType; } }
    public GameObject ItemPrefab
    { get { return itemPrefab; } }

}
