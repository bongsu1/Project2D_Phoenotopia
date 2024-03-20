using System.Collections;
using UnityEngine;

public class WorldEnemySpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] WorldEnemy worldEnemyPrefab;
    [SerializeField] Vector2 minRange;
    [SerializeField] Vector2 maxRange;
    [SerializeField] float spawnTime;

    private WorldEnemy[] enemies;
    private Coroutine spawnRoutine;
    private WaitForSeconds spawnWait;

    [ContextMenu("CreatePool")]
    public void CreatEnemyPool()
    {
        enemies = new WorldEnemy[5];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = Instantiate(worldEnemyPrefab);
            enemies[i].gameObject.SetActive(false);
        }

        spawnWait = new WaitForSeconds(spawnTime);
    }

    [ContextMenu("Spawn Start")]
    public void StartSpawnRoutine()
    {
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return spawnWait;

        while (true)
        {
            float xSign = Random.Range(0, 2) == 0 ? 1f : -1f;
            float ySign = Random.Range(0, 2) == 0 ? 1f : -1f;
            spawnPoint.position = new Vector2(xSign * Random.Range(minRange.x / 2, maxRange.x / 2), ySign * Random.Range(minRange.y / 2, maxRange.y / 2));
            for (int i = 0; i < enemies.Length; i++)
            {
                if (!enemies[i].gameObject.activeSelf)
                {
                    enemies[i].transform.position = spawnPoint.position;
                    enemies[i].gameObject.SetActive(true);
                    break;
                }
            }

            yield return spawnWait;
        }
    }

    [ContextMenu("Spawn Stop")]
    public void StopSpawnRotine()
    {
        if (spawnRoutine == null)
            return;

        StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, minRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, maxRange);
    }
}
