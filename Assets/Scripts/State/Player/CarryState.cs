using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryState : PlayerState
{
    BoxCollider2D boxColl;
    Vector2 offset;
    SpriteRenderer boxRender;
    Rigidbody2D boxRigid;

    int pressCount; // 던지기나 놓기 키를 누르면 스테이트 변경

    public override void Enter()
    {
        boxRigid = player.Box.gameObject.GetComponent<Rigidbody2D>();
        boxRigid.mass = 0f;

        boxColl = player.Box.gameObject.GetComponent<BoxCollider2D>();
        offset = new Vector2(0f, player.PlayerColl.size.y + (boxColl.size.y / 3));

        boxRender = player.Box.gameObject.GetComponent<SpriteRenderer>();
        boxRender.sortingOrder = 6;

        player.Box.transform.localPosition = offset; // 나중에 자연스럽게 바꾸기

        pressCount = 1;
    }

    public override void Update()
    {
        if (pressCount <= 0)
            return;

        if (player.IsGrounded && player.Input.actions["Jump"].IsPressed() && player.Input.actions["Jump"].triggered)
        {
            Jump();
        }

        if (!player.IsGrounded && player.Rigid.velocity.y > 0f)
        {
            player.Animator.Play("BoxJump");
        }
        else if (!player.IsGrounded && player.Rigid.velocity.y <= 0f)
        {
            player.Animator.Play("BoxFall");
        }
        else if (player.MoveDir.x != 0)
        {
            player.transform.localScale = new Vector3(player.MoveDir.x, 1, 1);

            player.Animator.Play("BoxCarry");
        }
        else if (player.MoveDir.x == 0)
        {
            player.Animator.Play("BoxIdle");
        }


        if (player.IsGrounded && player.Input.actions["Grab"].IsPressed() && player.Input.actions["Grab"].triggered)
        {

            offset = new Vector2(Mathf.Abs(boxColl.size.x + player.PlayerColl.size.x) * 0.5f, (boxColl.size.y * 0.5f) + 0.01f);
            player.Animator.Play("BoxDown");
            pressCount--;
        }

        player.Box.transform.localPosition = offset;

        if (player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered)
        {
            boxRigid.mass = 5f;
            player.Box.transform.parent = null;
            boxRigid.velocity = new Vector2(player.transform.localScale.x * player.ThrowPower * 2, player.ThrowPower);
            player.Animator.Play("BoxThrow");
            pressCount--;
        }
    }

    public override void FixedUpdate()
    {
        Move();
    }

    public override void Exit()
    {
        boxRigid.mass = 5f;
        boxRender.sortingOrder = 2;
        player.Box.transform.parent = null;
        player.Box = null;
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

    public CarryState(Player player)
    {
        this.player = player;
    }
}
