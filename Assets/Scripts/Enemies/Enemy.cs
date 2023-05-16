using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected float swingCooldown;
    [SerializeField] protected float moveDelay;
    private bool canStrike = true;
    private GameObject targetPlayer;
    private LayerMask playerMask = 3;

    //Remember this can be triggered by the scene view camera
    private void OnBecameVisible()
    {
        Debug.Log("I'm Visible");
    }
    
    private void OnEnable()
    {
        //reset target player
    }

    //check for layer "Player" bitmask overlap sphere?
    //target closest player
    protected IEnumerator Move()
    {
        yield return new WaitForSeconds(moveDelay);
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
        Debug.Log("swing"); //Get playerData? component in collision and call victim.TakeDamage(int damage)?
        canStrike = true;
    }

    protected void GivePoints(int points)
    {
        //Will call a script in ___ to give the player "points"
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);
    }
}