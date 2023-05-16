using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour, IFloorItem
{
    //private float scoreAmount = 100f;
    private float healthAmount = 100f;
    [SerializeField] private bool canShoot = false;

    public void HandlePickup(PlayerController player)
    {
        player.classData.CurHealth += healthAmount;
        //player.classData.Score += scoreAmount;

        GameObject scoreCanvas = ObjectPooler.Instance.GetPooledObject("ScoreCanvas");
        if (scoreCanvas != null)
        {
            scoreCanvas.transform.position = transform.position + (Vector3.up * 2);
            scoreCanvas.SetActive(true);
        }
        Destroy(gameObject);
    }

    public void HandleShot(PlayerController player)
    {
        if(canShoot)
        {
            Destroy(gameObject);
            NaratorManager.Instance.AssignNarationAndPlay(PromptType.PlayerShotFood, 3, player.classData);
        }
    }
}
