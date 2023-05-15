using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected int pointValue;
    [SerializeField] protected float swingCooldown;
    [SerializeField] protected float moveSpeed;
    private bool canStrike = true;

    protected IPlayer warrior;
    protected IPlayer valkyrie;
    protected IPlayer wizard;
    protected IPlayer elf;
    private GameObject targetPlayer;
    protected Collider[] playerColliders = new Collider[4];
    protected LayerMask playerMask = 3;
    [SerializeField] protected float detectionRadius = 100f;

    //This can be triggered by the scene view camera as well as the main camera
    private void OnBecameVisible()
    {
        //Debug.Log("I'm Visible");
        findTargetPlayer();
    }
    
    private void OnEnable()
    {
        findTargetPlayer();
    }

    protected virtual void findTargetPlayer()
    {
        int playerCount = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, playerColliders, playerMask);

        for(short c = 0; c < playerCount; c++)
        {
            IPlayer currentPlayer = playerColliders[c].gameObject.GetComponent<IPlayer>();
            switch (currentPlayer.ClassType)
            {
                case ClassEnum.Warrior:
                    warrior = currentPlayer;
                    break;
                case ClassEnum.Valkyrie:
                    valkyrie = currentPlayer;
                    break;
                case ClassEnum.Wizard:
                    wizard = currentPlayer;
                    break;
                case ClassEnum.Elf:
                    elf = currentPlayer;
                    break;
                default:
                    Debug.Log("Class Sort Defauted in Enemy.cs");
                    break;
            }
        }

        targetPlayer = playerColliders[(Random.Range(0, playerCount))].gameObject;
        StartCoroutine(Move());
    }

    protected virtual IEnumerator Move()
    {
        while(transform.position.x != targetPlayer.transform.position.x || transform.position.z != targetPlayer.transform.position.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, Time.deltaTime*moveSpeed);
        }

        yield return null;
    }

    protected virtual IEnumerator Attack(Collision victim)
    {
        canStrike = false;
        victim.gameObject.GetComponent<Player>().CurHealth -= damage;
        yield return new WaitForSeconds(swingCooldown);
        canStrike = true;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3 && canStrike)
        {
            StartCoroutine(Attack(collision));
        }
        else
        {
            switch(collision.gameObject.tag)
            {
                case "Bullet/Warrior":
                    if(TakeDamage(100))
                    {
                        warrior.Score += pointValue;
                    }
                    break;
                case "Bullet/Wizard":
                    if (TakeDamage(200))
                    {
                        wizard.Score += pointValue;
                    }
                    break;
                case "Bullet/Valkyrie":
                    if (TakeDamage(120))
                    {
                        valkyrie.Score += pointValue;
                    }
                    break;
                case "Bullet/Elf":
                    if (TakeDamage(100))
                    {
                        elf.Score += pointValue;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public bool TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    public virtual void Die()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    public void GivePoints(Player attackingPlayer)
    {
        attackingPlayer.Score += pointValue;
    }
}