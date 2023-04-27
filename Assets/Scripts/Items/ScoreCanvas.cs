using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCanvas : MonoBehaviour
{
    private float waitTime = 4f;
    private float curTime = 0;
    private Text scoreText;
    private float baseAlpha;

    private void Start()
    {
        scoreText = GetComponentInChildren<Text>();
        baseAlpha = scoreText.color.a;
    }
    private void OnEnable()
    {
        StartCoroutine(DisplayScore());
    }

    private IEnumerator DisplayScore()
    {
        
        yield return new WaitForSeconds(2f);
        //Reduce text alpha till 0
        while(curTime < waitTime)
        {
            curTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(scoreText.color.a, 0, curTime / waitTime);
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, newAlpha);
            yield return null;
        }

        //Set alpha back to original
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, baseAlpha);
        //Disable game object
        gameObject.SetActive(false);
        yield return null;

    }
}
