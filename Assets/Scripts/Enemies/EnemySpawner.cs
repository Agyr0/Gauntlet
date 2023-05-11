using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
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
}