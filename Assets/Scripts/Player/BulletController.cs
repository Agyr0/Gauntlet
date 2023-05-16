using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private ScreenBorder screenBorder;
    private CapsuleCollider capsuleCollider;
    public PlayerController m_player;
    private string m_class;

    private void Start()
    {
        screenBorder = new ScreenBorder();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (screenBorder.IsOutside(transform, capsuleCollider.radius, capsuleCollider.radius))
            //Destroy(gameObject);
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<IFloorItem>() != null)
        {
            other.gameObject.GetComponent<IFloorItem>().HandleShot(m_player);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IFloorItem>() != null)
        {
            collision.gameObject.GetComponent<IFloorItem>().HandleShot(m_player);
        }
    }
}
