using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {

        }
    }
}
