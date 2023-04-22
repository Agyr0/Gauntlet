using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[System.Serializable]
public class PlayerInventory
{
    public PlayerController myPlayer;
    [HideInInspector]
    public ClassData myPlayerClass;
    [HideInInspector]
    public ClassEnum myPlayerClassType;
    private int maxItems = 12;

    public List<ItemData> myItems = new List<ItemData>();

    public PlayerInventory(PlayerController player)
    {
        myPlayer = player;
        myPlayerClass = player.classData;
        myPlayerClassType = myPlayerClass.ClassType;
        myItems.Capacity = maxItems;
    }
}

public class InventoryManager : Singleton<InventoryManager>
{
    [ListElementTitle("myPlayerClassType")]
    public List<PlayerInventory> playersInventories = new List<PlayerInventory>();


    public void LinkInventory(PlayerController player)
    {
        PlayerInventory inventory = new PlayerInventory(player);
        playersInventories.Add(inventory);
        player.m_inventory = inventory;
    }

}

