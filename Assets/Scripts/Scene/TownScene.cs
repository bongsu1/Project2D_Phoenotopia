using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScene : BaseScene
{
    [SerializeField] Player player;
    [SerializeField] CinemachineVirtualCamera[] virtualCamera; // 0:RoomCamera 1:PlayerFollowCamera

    public override IEnumerator LoadingRoutine()
    {
        if (exitPoint == 0) // 시작하거나 죽었을때
        {
            player.StartGame();
            virtualCamera[0].Priority = 10;
            virtualCamera[1].Priority = 5;
        }
        else
        {
            virtualCamera[0].Priority = 5;
            virtualCamera[1].Priority = 10;
        }
        player.transform.position = startPoint[exitPoint].position; // 0:bed 1:townLeft 2:townRight
        yield return null;
    }

    // 씬 전환
    public void WorldSceneLoad(int point)
    {
        exitPoint = point;
        Manager.Scene.LoadScene("WorldScene");
    }
}
