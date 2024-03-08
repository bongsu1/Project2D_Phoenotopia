using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseState : PlayerState
{
    // 장착한 아이템 종류에 따라 다르게 업데이트 (무기, 음식)
    // 임시로 새총만 구현

    float time;

    public override void Enter()
    {
        player.Animator.Play("SlingShot");
        player.AimRotateAngle = -10;
        player.Rigid.velocity = Vector3.zero;
    }

    public override void Update()
    {
        time += Time.deltaTime / player.useTime;
        // z축 초기값 -10
        player.AimRotateAngle -= 60 / player.useTime;
        // z축 마지막 50
        if (time > 1.2f)
        {
            player.Animator.Play("SlingShotEnd");
        }
    }

    public override void Exit()
    {
        time = 0;
    }

    public UseState(Player player)
    {
        this.player = player;
    }
}
