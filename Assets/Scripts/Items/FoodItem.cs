using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour, IFloorItem
{

    [SerializeField] private float healthAmount = 100f;


    public void HandlePickup(PlayerController player)
    {
        player.classData.CurHealth += healthAmount;
        Destroy(gameObject);
    }

    public void HandleShot()
    {
        Destroy(gameObject);
    }
}
