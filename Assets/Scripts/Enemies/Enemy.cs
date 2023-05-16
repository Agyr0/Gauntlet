using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected int pointValue;
    [SerializeField] protected float swingCooldown;
    [SerializeField] protected float moveSpeed;
    protected bool canStrike = true;

    protected PlayerController warrior;
    protected PlayerController valkyrie;
    protected PlayerController wizard;
    protected PlayerController elf;
    [SerializeField] protected GameObject targetPlayer;
    [SerializeField] protected IPlayer targetPlayerPlayer;
    protected PlayerController[] myPlayers = new PlayerController[4];
    protected LayerMask playerMask = 3;
    [SerializeField] protected float detectionRadius = 100f;
    private Vector3 tempPos;

    //This can be triggered by the scene view camera as well as the main camera
    private void OnBecameVisible()
    {
        //Debug.Log("I'm Visible");
        findTargetPlayer();
    }
    
    /*
    private void OnEnable()
    {
        findTargetPlayer();
    }
    */
    protected virtual void findTargetPlayer()
    {
        myPlayers = FindObjectsOfType<PlayerController>();
        //identifyPlayer();
        targetPlayer = myPlayers[(Random.Range(0, 1))].gameObject;
        targetPlayerPlayer = targetPlayer.GetComponent<PlayerController>().player;
        StartCoroutine(Move());
    }

    private void identifyPlayer()
    {
        for(short c = 0; myPlayers[c] != null; c++)
        {
            PlayerController currentPlayer = myPlayers[c];
            switch (currentPlayer.classData.ClassType)
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
                    Debug.Log("Class Sort Defaulted in Enemy.cs");
                    break;
            }
        }
    }

    protected virtual IEnumerator Move()
    {
        if(transform.position.x != targetPlayer.transform.position.x || transform.position.z != targetPlayer.transform.position.z)
        {
            tempPos = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, Time.deltaTime * moveSpeed);
            tempPos.y = Mathf.Clamp(tempPos.y, 1f, 1f);
            transform.position = tempPos;
        }

        yield return new WaitForFixedUpdate();
        StartCoroutine(Move());
    }

    protected virtual IEnumerator Attack(Collision victim)
    {
        canStrike = false;
        victim.gameObject.GetComponent<PlayerController>().classData.CurHealth -= damage;
        yield return new WaitForSeconds(swingCooldown);
        canStrike = true;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3 && canStrike)
        {
            StartCoroutine(Attack(collision));
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Bullet/Warrior":
                if(TakeDamage(100))
                {
                    other.gameObject.SetActive(false);
                    //targetPlayerPlayer.Score += pointValue;
                }
                break;
            case "Bullet/Wizard":
                if (TakeDamage(200))
                {
                    other.gameObject.SetActive(false);
                    //targetPlayerPlayer.Score += pointValue;
                }
                break;
            case "Bullet/Valkyrie":
                if (TakeDamage(120))
                {
                    other.gameObject.SetActive(false);
                    //targetPlayerPlayer.Score += pointValue;
                }
            break;
            case "Bullet/Elf":
                if (TakeDamage(100))
                {
                    other.gameObject.SetActive(false);
                    //targetPlayerPlayer.Score += pointValue;
                }
            break;
            default:
                break;
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

    public void GivePoints(PlayerController attackingPlayer)
    {
        attackingPlayer.classData.Score += pointValue;
    }
}