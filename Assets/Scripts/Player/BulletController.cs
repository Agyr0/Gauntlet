using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private void Start()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
