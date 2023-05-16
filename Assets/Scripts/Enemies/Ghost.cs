using System.Collections;
using UnityEngine;

public class Ghost : Enemy
{
    protected override IEnumerator Move()
    {
        while (transform.position.x != targetPlayer.transform.position.x || transform.position.z != targetPlayer.transform.position.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, Time.deltaTime * moveSpeed);
        }

        yield return null;
    }

    protected override IEnumerator Attack(Collision victim)
    {
        canStrike = false;
        victim.gameObject.GetComponent<Player>().CurHealth -= damage;
        yield return new WaitForSeconds(swingCooldown);
        canStrike = true;
    }
}
