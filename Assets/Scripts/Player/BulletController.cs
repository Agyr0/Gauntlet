using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private ScreenBorder screenBorder;
    private CapsuleCollider capsuleCollider;

    private void Start()
    {
        screenBorder = new ScreenBorder();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (screenBorder.IsOutside(transform))
            Destroy(gameObject);
    }
}
