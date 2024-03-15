using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScene : BaseScene
{
    [SerializeField] WorldPlayer player;
    [SerializeField] Transform battlePoint;
    [SerializeField] WorldEnemySpawner spawner;

    public override IEnumerator LoadingRoutine()
    {
        battlePoint.position = battlePosition;
        player.transform.position = startPoint[exitPoint].position; // 0: Monster 1:TownLeft 2:TownRight
        yield return null;
        spawner.CreatEnemyPool();
        yield return null;
        spawner.StartSpawnRoutine();
        yield return null;
    }

    public void TownSceneLoad()
    {
        Manager.Scene.LoadScene("TownScene");
    }

    public void BattleSceneLoad(Vector2 enemyPosition)
    {
        battlePosition = enemyPosition;
        Manager.Scene.LoadScene("BattleScene");
    }
}
