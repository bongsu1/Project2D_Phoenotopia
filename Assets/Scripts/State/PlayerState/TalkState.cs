using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : PlayerState
{
    public override void Enter()
    {
        player.Talk();
        player.Input.actions["Move"].Disable();

        player.Animator.Play("Idle");
        player.Animator.SetFloat("MoveSpeed", 0f);
    }

    public override void FixedUpdate()
    {
        player.Rigid.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        player.Input.actions["Move"].Enable();
    }

    public override void Transition()
    {
        if (!player.OnTalk)
        {
            ChangeState(Player.State.Normal);
        }
    }

    public TalkState(Player player)
    {
        this.player = player;
    }
}
