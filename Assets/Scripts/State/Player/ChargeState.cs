using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : PlayerState
{
    private bool isChargeAttackSuccesed;

    public override void Enter()
    {
        player.Animator.Play("Charging");
    }

    public override void Update()
    {
        // 차지중
        if ((player.ChargeTime < 1f) && (Mathf.Abs(player.MoveDir.x) < 0.1f) && player.IsGrounded && player.Input.actions["Attack"].IsPressed())
        {
            player.Animator.Play("Charging");
            player.ChargeTime += Time.deltaTime;
            player.Animator.SetFloat("ChargedTime", player.ChargeTime);
        }
        // 차지완료
        else if ((Mathf.Abs(player.MoveDir.x) < 0.1f) && player.IsGrounded && player.Input.actions["Attack"].IsPressed())
        {
            player.Animator.Play("Charged");
            player.Animator.SetFloat("ChargedTime", player.ChargeTime);
        }
        // 차지중 이동
        else if ((player.ChargeTime < 1f) && (Mathf.Abs(player.MoveDir.x) > 0.1f) && player.IsGrounded && player.Input.actions["Attack"].IsPressed())
        {
            player.Animator.Play("ChargingWalk");
            player.ChargeTime += Time.deltaTime;
            player.Animator.SetFloat("ChargedTime", player.ChargeTime);

            if (player.MoveDir.x != 0)
            {
                player.transform.localScale = new Vector3(player.MoveDir.x, 1, 1);
            }
        }
        // 차지완료 이동
        else if ((Mathf.Abs(player.MoveDir.x) > 0.1f) && player.IsGrounded && player.Input.actions["Attack"].IsPressed())
        {
            player.Animator.Play("ChargedWalk");
            player.Animator.SetFloat("ChargedTime", player.ChargeTime);

            if (player.MoveDir.x != 0)
            {
                player.transform.localScale = new Vector3(player.MoveDir.x, 1, 1);
            }
        }
        // 차지가 완료되고 키를 때면 차지공격
        else if (player.IsGrounded && !player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered && (player.ChargeTime >= 1f) && (Manager.Data.Stamina >= player.UseStamina))
        {
            player.Animator.Play("ChargeAttack");
            player.Input.actions["Attack"].Disable();
            player.SFX.PlaySFX(PlayerSoundManager.SFX.ChargeAttack);

            isChargeAttackSuccesed = true;
            Manager.Data.Stamina -= player.UseStamina;
            Manager.Data.StartStaminaRegenRoutine();
        }
    }

    public override void FixedUpdate()
    {
        Move();
    }

    public override void Exit()
    {
        player.ChargeTime = 0;
        isChargeAttackSuccesed = false;
        player.Input.actions["Attack"].Enable();
    }

    private void Move()
    {
        float target = player.MoveDir.x * player.MoveSpeed;
        float diffSpeed = target - player.Rigid.velocity.x;
        player.Rigid.AddForce(Vector2.right * diffSpeed * player.Accel);
    }

    public override void Transition()
    {
        if (!player.IsGrounded)
        {
            ChangeState(Player.State.Jump);
        }
        // 차지 완료가 되기 전에 때면 취소 + 스태미나가 부족해도 취소
        else if (!isChargeAttackSuccesed && !player.Input.actions["Attack"].IsPressed() && (player.ChargeTime < 1f || (Manager.Data.Stamina < player.UseStamina)))
        {
            ChangeState(Player.State.Normal);
        }
    }

    public ChargeState(Player player)
    {
        this.player = player;
    }
}
