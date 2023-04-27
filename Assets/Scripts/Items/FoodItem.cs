using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour, IFloorItem
{

    private float healthAmount = 100f;

    public void HandlePickup(PlayerController player)
    {
        player.classData.CurHealth += healthAmount;
        GameObject scoreCanvas = ObjectPooler.Instance.GetPooledObject("ScoreCanvas");
        if (scoreCanvas != null)
        {
            scoreCanvas.transform.position = transform.position + (Vector3.up * 2);
            scoreCanvas.SetActive(true);
        }
        Destroy(gameObject);
    }

    public void HandleShot()
    {
        Destroy(gameObject);
    }
}
