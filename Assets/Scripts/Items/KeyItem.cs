using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour, IFloorItem
{
    public ItemData data;

    public void HandlePickup(PlayerController player)
    {
        InventoryManager.Instance.AddItemToInventory(player, data);
        gameObject.SetActive(false);
    }

    public void HandleShot(PlayerController player)
    {
        
    }
}
