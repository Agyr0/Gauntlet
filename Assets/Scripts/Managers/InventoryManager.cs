using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


[System.Serializable]
public class PlayerInventory
{
    public PlayerController myPlayer;
    [HideInInspector]
    public ClassData myPlayerClass;
    [HideInInspector]
    public ClassEnum myPlayerClassType;
    public int maxItems = 12;

    public List<ItemData> myItems = new List<ItemData>();
    public List<GameObject> uiItems = new List<GameObject>();

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

    /// <summary>
    /// Add <paramref name="item"/> to <paramref name="player"/>'s inventory if it is not full "<see cref="PlayerInventory.maxItems"/>"
    /// <list type=""/>
    /// Returns true if item was added
    /// </summary>
    /// <param name="player"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItemToInventory(PlayerController player, ItemData item)
    {
        //If inventory is not full
        if (player.m_inventory.myItems.Count < player.m_inventory.maxItems)
        {
            //Add item and return true
            player.m_inventory.myItems.Add(item);
            player.m_inventory.uiItems.Add(item.UIPrefab);
            AddItemToUI(player, item, true);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Remove <paramref name="item"/> from <paramref name="player"/>'s inventory if it is not empty
    /// <para>
    /// Returns true if item was removed
    /// </para>
    /// </summary>
    /// <param name="player"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool RemoveItemFromInventory(PlayerController player, ItemData item)
    {
        //If there is an item to remove
        if (player.m_inventory.myItems.Count > 0)
        {
            //Remove it and return false
            player.m_inventory.myItems.Remove(item);
            player.m_inventory.uiItems.Remove(item.UIPrefab);

            AddItemToUI(player, item, false);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds <paramref name="item"/> to <paramref name="player"/> inventory 
    /// <para>
    /// Used for adding or removing items from inventory based on <paramref name="add"/>
    /// </para>
    /// </summary>
    /// <param name="player"></param>
    /// <param name="item"></param>
    /// <param name="add"></param>
    public void AddItemToUI(PlayerController player,ItemData item, bool add)
    {
        GameObject _item = ObjectPooler.Instance.GetPooledObject(item.itemPrefab.tag);
        if (add)
        {
            switch (player.classData.ClassType)
            {
                case ClassEnum.Warrior:
                    if (_item != null)
                    {
                        _item.transform.SetParent(UIManager.Instance.warriorInventory.transform);
                    }

                    break;
                case ClassEnum.Valkyrie:
                    if (_item != null)
                    {
                        _item.transform.SetParent(UIManager.Instance.valkyrieInventory.transform);
                    }
                    break;
                case ClassEnum.Wizard:
                    if (_item != null)
                    {
                        _item.transform.SetParent(UIManager.Instance.wizzardInventory.transform);
                    }
                    break;
                case ClassEnum.Elf:
                    if (_item != null)
                    {
                        _item.transform.SetParent(UIManager.Instance.elfInventory.transform);
                    }
                    break;
                default:
                    break;
            }
            _item.transform.SetAsLastSibling();
            _item.SetActive(true);
            Debug.Log(_item, _item);

        }
        else
        {
            _item.SetActive(false);
            _item.transform.SetParent(null);
            Debug.Log(_item, _item);
        }

    }
}

