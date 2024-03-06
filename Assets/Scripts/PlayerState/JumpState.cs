using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class JumpState : PlayerState
{
    public override void Enter()
    {
        if (player.MoveDir.y < 0)
        {
            player.Animator.Play("Fall");
        }
        else
        {
            player.Animator.Play("Jump");
        }
    }

    public override void Update()
    {
        player.Animator.SetFloat("FallSpeed", player.Rigid.velocity.y);
    }

    public override void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float target = player.MoveDir.x * player.MoveSpeed;
        float diffSpeed = target - player.Rigid.velocity.x;
        player.Rigid.AddForce(Vector2.right * diffSpeed * player.Accel);
    }

    public override void Transition()
    {
        if (player.IsGrounded)
        {
            ChangeState(Player.State.Normal);
        }
        if (player.IsLadder && player.MoveDir.y > 0f)
        {
            ChangeState(Player.State.Climb);
        }
    }

    public JumpState(Player player)
    {
        this.player = player;
    }
}
