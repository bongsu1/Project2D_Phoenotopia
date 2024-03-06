using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

// 계속 구현이 안되면 뺄예정
public class SleepState : PlayerState
{
    public override void Enter()
    {
        player.Input.actions["Move"].Disable();
        player.Animator.Play("Sleep");
    }

    public override void Exit()
    {
        player.Animator.Play("WakeUp"); // 애니메이션이 재생되야 하는데 코루틴을 쓸수 없음
        player.Input.actions["Move"].Enable();
    }

    public override void Transition()
    {
        if (player.Input.actions["Jump"].IsPressed() && player.Input.actions["Jump"].triggered)
        {
            ChangeState(Player.State.Normal);
        }
    }

    public SleepState(Player player)
    {
        this.player = player;
    }
}
