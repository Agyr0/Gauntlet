using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScroller : MonoBehaviour
{
    private Image _img;
    private float myTime = 0;
    private void OnEnable()
    {
        Debug.Log("Started Scrolling", this.gameObject);
        if (_img == null)
            _img = GetComponent<Image>();
        _img.material.SetFloat("_UnscaledTime", 0);
        myTime = 0;
        StartCoroutine(Scroll());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _img.material.SetFloat("_UnscaledTime", 0);
        myTime = 0;
        Debug.Log("Stopped scrolling");
    }

    private IEnumerator Scroll()
    {
        //Debug.Log("Started Scrolling");
        while (true)
        {
            myTime += Time.unscaledDeltaTime;
            _img.material.SetFloat("_UnscaledTime", myTime);
            yield return null;
        }
    }
}
