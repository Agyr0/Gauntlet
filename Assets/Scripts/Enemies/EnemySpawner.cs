using System.Collections;
using UnityEngine;

public class EnemySpawner : Enemy
{
    [SerializeField] private float spawnDelay;
    //The ordinal directions around the EnemySpawner
    [SerializeField] private SpawnPoint[] spawnPoints = new SpawnPoint[8];

    private void OnBecameVisible()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for(short c = 0; c < 8; c++)
        {
            if(!spawnPoints[c].occupied)
            {
                spawnPoints[c].SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }
            else
            {
                yield return new WaitForSeconds(spawnDelay / 10);
            }
        }

        StartCoroutine(Spawn());
    }

    /*
    protected override void findTargetPlayer()
    {
        int playerCount = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, playerColliders, playerMask);

        for (short c = 0; c < playerCount; c++)
        {
            PlayerController currentPlayer = playerColliders[c].gameObject.GetComponent<PlayerController>();
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
                    Debug.Log("Class Sort Defauted in Enemy.cs");
                    break;
            }
        }
    }
    */
}