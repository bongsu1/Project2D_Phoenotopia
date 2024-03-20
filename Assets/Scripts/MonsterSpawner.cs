using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] Transform[] spawnPoint;
    [SerializeField] Transform player;

    [ContextMenu("Spawn")]
    public void SpawnMonster()
    {
        int ranIndex;

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            ranIndex = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[ranIndex], spawnPoint[i].position, spawnPoint[i].rotation);
        }
    }
}
