using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

// 계속 구현이 안되면 뺄예정 // 해결
public class SleepState : PlayerState
{
    public override void Enter()
    {
        player.Animator.Play("Sleep");
    }

    public override void Update()
    {
        if (player.Input.actions["Jump"].IsPressed() && player.Input.actions["Jump"].triggered) // 코루틴 대신 애니메이션에서 ChangeState를 호출
        {
            player.Input.actions["Jump"].Disable();
            player.Animator.Play("WakeUp");
            player.SFX.PlaySFX(PlayerSoundManager.SFX.WakeUp);
        }
    }

    public override void Exit()
    {
        player.Input.actions["Jump"].Enable();
    }

    public SleepState(Player player)
    {
        this.player = player;
    }
}
