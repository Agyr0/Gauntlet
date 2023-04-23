using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureItem : MonoBehaviour, IFloorItem
{

    private float scoreAmount = 100f;


    public void HandlePickup(PlayerController player)
    {
        player.classData.Score += scoreAmount;
        Destroy(gameObject);
    }

    public void HandleShot()
    {
        Destroy(gameObject);
    }
}
