using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool occupied = false;
    [SerializeField] private EnemySpawner parentSpawner;
    [SerializeField] private GameObject enemyToSpawn;

    public void SpawnEnemy()
    {
        GameObject enemy = ObjectPooler.Instance.GetPooledObject(enemyToSpawn.tag);
        enemy.transform.position = transform.position;
        enemy.SetActive(true);
        occupied = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            occupied = false;
        }
    }
}
