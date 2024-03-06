using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckState : PlayerState
{
    public override void Enter()
    {
        if (Mathf.Abs(player.Rigid.velocity.x) > 2.5f)
        {
            player.Animator.Play("Roll");
        }
        else
        {
            player.Animator.Play("Duck");
        }
        player.PlayerColl.offset = new Vector2(0f, 0.3f);
        player.PlayerColl.size = new Vector2(0.45f, 0.6f);
    }

    public override void Update()
    {
        if (player.MoveDir.x != 0)
        {
            player.transform.localScale = new Vector3(player.MoveDir.x, 1, 1);
        }
    }

    public override void FixedUpdate()
    {
        Move();
    }

    public override void Exit()
    {
        player.PlayerColl.offset = new Vector2(0f, 0.4f);
        player.PlayerColl.size = new Vector2(0.45f, 0.8f);
    }

    private void Move()
    {
        if(Mathf.Abs(player.Rigid.velocity.x)> 2.1f)
        {
            player.Animator.Play("Roll");
        }
        else if (Mathf.Abs(player.MoveDir.x) > 0)
        {
            player.Animator.Play("DuckMove"); // +움직이고 있지 않을 때 애니메이션 정지
        }

        float target = player.MoveDir.x * player.MoveSpeed;
        float diffSpeed = target - player.Rigid.velocity.x;
        player.Rigid.AddForce(Vector2.right * diffSpeed * player.Accel);
    }

    public override void Transition()
    {
        if (!player.IsDucking) // + 머리위에 천장이 없으면
        {
            ChangeState(Player.State.Normal);
        }
        else if (!player.IsGrounded)
        {
            ChangeState(Player.State.Jump);
        }
    }

    public DuckState(Player player)
    {
        this.player = player;
    }
}
