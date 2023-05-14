using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : MonoBehaviour, IFloorItem
{
    public ItemData data;

    public void HandlePickup(PlayerController player)
    {
        InventoryManager.Instance.AddItemToInventory(player, data);
        gameObject.SetActive(false);
    }

    public void HandleShot()
    {
        GameManager.Instance.UsePotion(true);
        gameObject.SetActive(false);
    }
}
