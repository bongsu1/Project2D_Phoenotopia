using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] Transform[] spawnPoint;
    [SerializeField] Transform player;

    public void SpawnMonster()
    {
        int ranIndex;

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            ranIndex = Random.Range(0, enemyPrefab.Length);
            float spawnDir = player.position.x - spawnPoint[i].position.x;
            if (spawnDir < 0)
            {
            }
            Instantiate(enemyPrefab[ranIndex], spawnPoint[i].position, spawnPoint[i].rotation);
        }
    }
}
