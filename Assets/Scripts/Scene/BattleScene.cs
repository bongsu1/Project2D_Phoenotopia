using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] MonsterSpawner spawner;

    public override IEnumerator LoadingRoutine()
    {
        exitPoint = 0;
        spawner.SpawnMonster();
        yield return null;
    }

    public void WorldSceneLoad()
    {
        Manager.Scene.LoadScene("WorldScene");
    }
}
