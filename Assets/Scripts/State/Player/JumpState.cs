using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class JumpState : PlayerState
{
    private float airTime;

    public override void Enter()
    {
        airTime = 0f;

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
        airTime += Time.deltaTime;

        if (player.Input.actions["Run"].IsPressed() && player.Input.actions["Run"].triggered)
        {
            Manager.Data.Stamina -= player.UseStamina;
            Manager.Data.StopStaminaRegenRoutine();
        }
        else if (player.Input.actions["Run"].IsPressed() && player.Rigid.velocity.magnitude > 0.01f)
        {
            Manager.Data.Stamina -= Time.deltaTime;
        }
        else if (!player.Input.actions["Run"].IsPressed() && player.Input.actions["Run"].triggered)
        {
            Manager.Data.StartStaminaRegenRoutine();
        }

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
        if (Manager.Data.Stamina >= player.UseStamina && player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered)
        {
            Manager.Data.StartStaminaRegenRoutine();
            ChangeState(Player.State.Attack);
        }
        else if (player.IsGrounded)
        {
            ChangeState(Player.State.Normal);
        }
        else if (player.IsLadder && (player.MoveDir.y > 0f) && (airTime > 0.3f))
        {
            ChangeState(Player.State.Climb);
        }
    }

    public JumpState(Player player)
    {
        this.player = player;
    }
}
