using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour, IFloorItem
{
    public ItemData data;

    public void HandlePickup(PlayerController player)
    {
        gameObject.SetActive(false);
        InventoryManager.Instance.AddItemToInventory(player, data);

    }

    public void HandleShot(PlayerController player)
    {
        
    }
}
