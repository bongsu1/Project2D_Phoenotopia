using UnityEngine;

public class CarryState : PlayerState
{
    Vector2 boxSize;
    Vector2 offset;

    int pressCount; // 던지기나 놓기 키를 누르면 스테이트 변경

    public override void Enter()
    {
        player.MoveSpeed = player.NormalSpeed;

        player.Box.Rigid.mass = 0f;

        boxSize = player.Box.BoxColl.size;
        offset = new Vector2(0f, player.PlayerColl.size.y + (boxSize.y / 3));

        player.Box.SpriteRender.sortingOrder = 6;

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

            offset = new Vector2(Mathf.Abs(boxSize.x + player.PlayerColl.size.x) * 0.5f, (boxSize.y * 0.5f) + 0.01f);
            player.Animator.Play("BoxDown");
            pressCount--;
        }

        player.Box.transform.localPosition = offset;

        if (player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered)
        {
            player.Box.Rigid.mass = 5f;
            player.Box.transform.parent = null;
            player.Box.Rigid.velocity = 
                new Vector2(player.transform.localScale.x * player.ThrowPower * 2f, player.ThrowPower);
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
        player.Box.Rigid.mass = 5f;
        player.Box.SpriteRender.sortingOrder = 2;
        player.Box.transform.parent = null;
        player.Box = null;
    }

    public CarryState(Player player) : base(player) { }
}
