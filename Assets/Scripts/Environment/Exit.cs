using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public Canvas winCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3 )
        {
            winCanvas.gameObject.SetActive(true);
        }
    }
}