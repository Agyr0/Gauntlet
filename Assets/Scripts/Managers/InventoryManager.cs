using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[System.Serializable]
public class PlayerInventory
{
    public PlayerController myPlayer;
    private int maxItems = 12;

    public List<GameObject> myObjects = new List<GameObject>();

    public PlayerInventory(PlayerController player)
    {
        myPlayer = player;
    }
}

public class InventoryManager : Singleton<InventoryManager>
{
    public List<PlayerInventory> playersInventories = new List<PlayerInventory>();


    public void LinkInventory(PlayerController player)
    {
        playersInventories.Add(new PlayerInventory(player));
    }

}

