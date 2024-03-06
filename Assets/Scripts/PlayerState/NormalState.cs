using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : PlayerState
{
    public override void Enter()
    {
        player.Animator.Play("Idle");
    }

    public override void Update()
    {
        if (player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered)
        {
            Attack();
        }

        player.Animator.SetFloat("MoveSpeed", Mathf.Abs(player.Rigid.velocity.x));

        if (player.MoveDir.x != 0)
        {
            player.transform.localScale = new Vector3(player.MoveDir.x, 1, 1);
        }
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

    private void Jump()
    {
        player.Rigid.velocity = new Vector2(player.Rigid.velocity.x, player.JumpSpeed);
    }

    private void Attack()
    {
        player.Animator.Play("Attack");
    }

    public override void Transition()
    {
        // 아래키를 누르고 있으면 DuckState
        if (player.MoveDir.y < -0.1f)
        {
            ChangeState(Player.State.Duck);
        }
        // Jump키를 누르면 점프후 JumpState
        else if (player.Input.actions["Jump"].IsPressed() && player.Input.actions["Jump"].triggered)
        {
            Jump();
            if (!player.IsGrounded)
            {
                ChangeState(Player.State.Jump);
            }
        }
        // 떨어지면 JumpState
        else if (!player.IsGrounded)
        {
            ChangeState(Player.State.Jump);
        }
        // 옆에 사다리가 있고 위키를 누르면 ClimbState
        else if (player.IsLadder && player.MoveDir.y > 0f)
        {
            ChangeState(Player.State.Climb);
        }
    }

    public NormalState(Player player)
    {
        this.player = player;
    }
}
