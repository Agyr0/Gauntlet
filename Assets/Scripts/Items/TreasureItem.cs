using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureItem : MonoBehaviour, IFloorItem
{

    private float scoreAmount = 100f;
    [SerializeField] private float scoreWaitTime = 2f;
    
    public void HandlePickup(PlayerController player)
    {
        player.classData.Score += scoreAmount;
        StartCoroutine(DisplayScorePrompt());
        Destroy(gameObject);
    }

    public void HandleShot()
    {
        Destroy(gameObject);
    }

    public IEnumerator DisplayScorePrompt()
    {
        float alpha = 255f;
        GameObject scoreCanvas = ObjectPooler.Instance.GetPooledObject("ScoreCanvas");
        Color textColor = scoreCanvas.GetComponentInChildren<Text>().color;
        if(scoreCanvas != null)
        {
            scoreCanvas.transform.position = transform.position + (transform.up * 2);
            scoreCanvas.SetActive(true);
        }

        yield return new WaitForSeconds(scoreWaitTime);

        while (textColor.a > 0)
        {


            textColor.a = textColor.a - .01f;

            scoreCanvas.GetComponentInChildren<Text>().color = textColor;
        }
        yield return null;
    }
}
