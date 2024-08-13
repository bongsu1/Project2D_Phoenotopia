using UnityEngine;

public class ChargeState : PlayerState
{
    private bool isChargeAttackSuccesed;

    public override void Enter()
    {
        player.MoveSpeed = player.ChargedMoveSpeed;

        player.Animator.Play("Charging");
    }

    public override void Update()
    {
        // ������
        if ((player.ChargeTime < 1f) && (Mathf.Abs(player.MoveDir.x) < 0.1f) && player.IsGrounded && player.Input.actions["Attack"].IsPressed())
        {
            player.Animator.Play("Charging");
            player.ChargeTime += Time.deltaTime;
            player.Animator.SetFloat("ChargedTime", player.ChargeTime);
        }
        // �����Ϸ�
        else if ((Mathf.Abs(player.MoveDir.x) < 0.1f) && player.IsGrounded && player.Input.actions["Attack"].IsPressed())
        {
            player.Animator.Play("Charged");
            player.Animator.SetFloat("ChargedTime", player.ChargeTime);
        }
        // ������ �̵�
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
        // �����Ϸ� �̵�
        else if ((Mathf.Abs(player.MoveDir.x) > 0.1f) && player.IsGrounded && player.Input.actions["Attack"].IsPressed())
        {
            player.Animator.Play("ChargedWalk");
            player.Animator.SetFloat("ChargedTime", player.ChargeTime);

            if (player.MoveDir.x != 0)
            {
                player.transform.localScale = new Vector3(player.MoveDir.x, 1, 1);
            }
        }
        // ������ �Ϸ�ǰ� Ű�� ���� ��������
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

    public override void Transition()
    {
        if (!player.IsGrounded)
        {
            ChangeState(Player.State.Jump);
        }
        // ���� �Ϸᰡ �Ǳ� ���� ���� ��� + ���¹̳��� �����ص� ���
        else if (!isChargeAttackSuccesed && !player.Input.actions["Attack"].IsPressed() && (player.ChargeTime < 1f || (Manager.Data.Stamina < player.UseStamina)))
        {
            ChangeState(Player.State.Normal);
        }
    }

    public ChargeState(Player player) : base(player) { }
}
