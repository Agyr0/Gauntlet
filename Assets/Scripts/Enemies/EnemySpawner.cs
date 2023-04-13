using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject enemyToSpawn;

    //Whether the ordinal directions around the gameObject are occupied
    private bool nw, n, ne, w, e, sw, s, se;

    private void OnBecameInvisible()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        GameObject enemy = ObjectPooler.Instance.GetPooledObject(enemyToSpawn.tag);

        if(enemy != null)
        {
            //enemy.transform.position = 
        }

        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(Spawn());
    }
}