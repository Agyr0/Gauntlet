using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFloorItem
{
    
    void HandlePickup(PlayerController player);

    void HandleShot();

}
