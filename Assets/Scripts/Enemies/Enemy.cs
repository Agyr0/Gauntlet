using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected float swingCooldown;
    private bool canStrike = true;
    private bool canMove;

    //Remember this can be triggered by the scene view camera
    private void OnBecameVisible()
    {
        Debug.Log("I'm Visible");

        canMove = true;
        //Move();
    }


    //check for layer "Player"
    protected IEnumerator Move()
    {
        while(canMove)
        {

        }

        yield return null;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(canStrike)
        {
            StartCoroutine(SwingCooldown(collision));
        }
    }

    IEnumerator SwingCooldown(Collision victim)
    {
        canStrike = false;
        yield return new WaitForSeconds(swingCooldown);
        Debug.Log("swing"); //Get playerData? component in collision and call its "TakeDamage(int)" passing this->damage
        canStrike = true;
    }

    protected void GivePoints(int points)
    {
        //Will call a script in ___ to give the player "points"
    }

    protected void TakeDamage(int damage)
    {
        health -= damage;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}