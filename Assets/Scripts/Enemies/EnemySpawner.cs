using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject enemyToSpawn;

    //The ordinal directions around the EnemySpawner
    [SerializeField] private SpawnPoint[] spawnPoints = new SpawnPoint[8];
    private Vector3 openPoint = new Vector3(0, 0, 0);
    private bool anyOpen = true;

    private void OnBecameVisible()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        GameObject enemy = ObjectPooler.Instance.GetPooledObject(enemyToSpawn.tag);
        FindOpenPoint();

        if(enemy != null && anyOpen)
        {
            enemy.transform.position = transform.position + openPoint;
            enemy.SetActive(true);
        }

        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(Spawn());
    }

    private void FindOpenPoint()
    {
        for (short c = 0; c < 8; c++)
        {
            if (!spawnPoints[c].occupied)
            {
                spawnPoints[c].occupied = true;
                openPoint = spawnPoints[c].transform.position;
                return;
            }
        }

        //This is currently immediately triggering before anything can spawn. Fix this first
        anyOpen = false;
    }
}