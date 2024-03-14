using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] MonsterSpawner spawner;

    public override IEnumerator LoadingRoutine()
    {
        spawner.SpawnMonster();
        yield return null;
    }
}
