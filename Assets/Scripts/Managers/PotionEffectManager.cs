using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffectManager : MonoBehaviour
{
    public ParticleSystem _particleSystem;

   
    private void OnEnable()
    {
        StartCoroutine(WaitTillDone());
    }

    private IEnumerator WaitTillDone()
    {
        yield return new WaitUntil(() => _particleSystem.isStopped == true);
        gameObject.SetActive(false);
    }
}
