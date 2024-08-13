using System.Collections;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] MonsterSpawner spawner;

    public override IEnumerator LoadingRoutine()
    {
        exitPoint = 0;
        statusRender.SetHp();
        Manager.Data.RefillStamina();
        yield return null;
        spawner.SpawnMonster();
        yield return null;
    }

    public void WorldSceneLoad()
    {
        Manager.Scene.LoadScene("WorldScene");
    }

    [SerializeField] bool debug;
    protected override void Start()
    {
        base.Start();

        if (debug)
            spawner.SpawnMonster();
    }
}
