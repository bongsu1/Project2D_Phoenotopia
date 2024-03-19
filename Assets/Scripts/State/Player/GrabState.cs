using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrabState : PlayerState
{
    float facing; // 바라보는 방향을 저장
    BoxCollider2D boxColl;
    Vector2 offset;
    Rigidbody2D boxRigid;

    public override void Enter()
    {
        player.Grab();
        if (player.Box != null)
        {
            player.Animator.Play("Push");
            player.Animator.speed = 0;

            facing = player.transform.localScale.x;

            boxColl = player.Box.gameObject.GetComponent<BoxCollider2D>();
            boxRigid = player.Box.gameObject.GetComponent<Rigidbody2D>();
            offset = new Vector2(Mathf.Abs(boxColl.size.x + player.PlayerColl.size.x) * 0.5f, (boxColl.size.y * 0.5f) + 0.01f);
            player.Box.transform.localPosition = offset;
            boxRigid.gravityScale = 0f;
        }
        else
        {
            player.Animator.Play("Grab");
        }
    }

    public override void Update()
    {
        if (player.Box == null)
            return;

        player.Box.transform.localPosition = offset;

        // 잡고있는 상태에서 위방향키를 누르고 있으면 들어올린다
        if (player.MoveDir.y > 0.1f)
        {
            if (player.Box.Mass > 4)
            {
                player.Animator.Play("BoxHeavy");
            }
            else
            {
                player.Animator.Play("BoxUp");
            }
            player.Animator.speed = 1f;
        }
        else if (player.MoveDir.x == 0)
        {
            player.Animator.Play("Push");
            player.Animator.speed = 0f;
        }
        // 바라보는 방향과 움직이는 방향이 같으면 밀기 애니메이션
        else if (Mathf.Abs(player.MoveDir.x - facing) < 0.1f)
        {
            player.Animator.Play("Push");
            player.Animator.speed = 1f;
        }
        // 반대는 끌기
        else if (Mathf.Abs(player.MoveDir.x - facing) > 1f)
        {
            player.Animator.Play("Pull");
            player.Animator.speed = 1f;
        }
    }

    public override void FixedUpdate()
    {
        Move();
    }

    public override void Exit()
    {
        player.Animator.speed = 1f;

        if (player.Box != null)
            boxRigid.gravityScale = 1f;
    }

    private void Move()
    {
        float target = player.MoveDir.x * player.MoveSpeed;
        float diffSpeed = target - player.Rigid.velocity.x;
        player.Rigid.AddForce(Vector2.right * diffSpeed * player.Accel);
    }

    public override void Transition()
    {
        if (player.Box == null)
            return;

        if (!player.Input.actions["Grab"].IsPressed() && player.Input.actions["Grab"].triggered)
        {
            ChangeState(Player.State.Normal);
            player.Box.transform.parent = null;
            player.Box = null;
        }
        else if (!player.IsGrounded)
        {
            ChangeState(Player.State.Jump);
            player.Box.transform.parent = null;
            player.Box = null;
        }
    }

    public GrabState(Player player)
    {
        this.player = player;
    }
}
