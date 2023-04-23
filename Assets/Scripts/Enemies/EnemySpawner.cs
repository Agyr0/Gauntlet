using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject enemyToSpawn;

    //Whether the ordinal directions around the gameObject are occupied
    private bool[] spawnPoints = {true, true, true, true, true, true, true, true};

    private void OnBecameVisible()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        GameObject enemy = ObjectPooler.Instance.GetPooledObject(enemyToSpawn.tag);

        if(enemy != null)
        {
            enemy.transform.position = transform.position + GetSpawnPoint();
            enemy.SetActive(true);
        }

        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(Spawn());
    }

    private int FindOpenPoint()
    {
        for (short c = 0; c < 8; c++)
        {
            if (spawnPoints[c])
            {
                spawnPoints[c] = false;
                Debug.Log(c.ToString());
                return c;
            }
        }

        return 8;
    }
    private Vector3 GetSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(0, 0, 0);
        switch (FindOpenPoint())
        {
            case 0:
                //North
                spawnPoint = new Vector3(0, 0, 1);
                break;
            case 1:
                //NorthEast
                spawnPoint = new Vector3(1, 0, 1);
                break;
            case 2:
                //East
                spawnPoint = new Vector3(1, 0, 0);
                break;
            case 3:
                //SouthEast
                spawnPoint = new Vector3(1, 0, -1);
                break;
            case 4:
                //South
                spawnPoint = new Vector3(0, 0, -1);
                break;
            case 5:
                //SouthWest
                spawnPoint = new Vector3(-1, 0, -1);
                break;
            case 6:
                //West
                spawnPoint = new Vector3(-1, 0, 0);
                break;
            case 7:
                //NorthWest
                spawnPoint = new Vector3(-1, 0, 1);
                break;
            case 8:
                spawnPoint = new Vector3(0, 0, 1);
                for(short c = 1; c < 8; c++)
                {
                    spawnPoints[c] = true;
                }
                break;
            default:
                Debug.Log("Switch default in GetSpawnPoint() in EnemySpawner.cs");
                break;
        }

        return spawnPoint;
    }
}