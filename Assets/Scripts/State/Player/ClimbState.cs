using UnityEngine;

public class ClimbState : PlayerState
{
    public override void Enter()
    {
        //Manager.UI.CloseTutorialUI();
        Manager.Data.StartStaminaRegenRoutine();
        player.Rigid.gravityScale = 0f;
        player.Animator.Play("Climb");
    }

    public override void Update()
    {
        if (!player.Input.actions["Move"].IsPressed())
        {
            player.Animator.speed = 0f;
        }
        else
        {
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
        player.Rigid.gravityScale = 1f;
    }

    protected override void Move()
    {
        player.Rigid.velocity = new Vector2(player.MoveDir.x, player.MoveDir.y).normalized * player.ClimbMoveSpeed;
    }

    public override void Transition()
    {
        if (player.Input.actions["Jump"].IsPressed() && player.Input.actions["Jump"].triggered)
        {
            if (player.MoveDir.y >= -0.1f)
            {
                Jump();
            }

            if (!player.IsGrounded)
            {
                ChangeState(Player.State.Jump);
            }
            else if (player.IsGrounded)
            {
                ChangeState(Player.State.Normal);
            }
        }
        else if (!player.IsLadder)
        {
            if (player.IsGrounded)
            {
                ChangeState(Player.State.Normal);
            }
            else
            {
                ChangeState(Player.State.Jump);
            }
        }
    }

    public ClimbState(Player player) : base(player) { }
}
