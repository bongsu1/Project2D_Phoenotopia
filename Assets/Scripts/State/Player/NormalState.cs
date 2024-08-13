using UnityEngine;

public class NormalState : PlayerState
{
    public override void Enter()
    {
        player.Animator.Play("Idle");
    }

    public override void Update()
    {
        player.MoveSpeed = player.NormalSpeed;

        if (player.Input.actions["Run"].IsPressed() && player.Input.actions["Run"].triggered)
        {
            Manager.Data.Stamina -= player.UseStamina * 0.5f;
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

        player.Animator.SetFloat("MoveSpeed", Mathf.Abs(player.Rigid.velocity.x));

        if (player.MoveDir.x != 0)
        {
            player.transform.localScale = new Vector3(player.MoveDir.x, 1, 1);
        }

        if (player.OnDoor && !player.OnEnter && (player.MoveDir.y > 0.1f))
        {
            player.EnterDoor();
        }
    }

    public override void FixedUpdate()
    {
        Move();
    }

    public override void Transition()
    {
        // ����Ű�� ������ AttackState
        if (Manager.Data.Stamina >= player.UseStamina && !player.OnNPC && player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered)
        {
            Manager.Data.StartStaminaRegenRoutine();
            ChangeState(Player.State.Attack);
        }
        // ���¹̳��� ������ ���¿��� ����Ű�� ������ ������ ChargeState
        else if (Manager.Data.Stamina < player.UseStamina && !player.OnNPC && player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered)
        {
            ChangeState(Player.State.Charge);
        }
        // NPC�տ��� ����Ű�� ������ TalkState
        else if (player.OnNPC && player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered)
        {
            ChangeState(Player.State.Talk);
        }
        // UseŰ�� ������ UseState
        else if (Manager.Data.Stamina >= player.UseStamina && player.Input.actions["Use"].IsPressed() && player.Input.actions["Use"].triggered)// (+�������� �����ϰ� ������, �������� �����Ǿ����� �߰�)
        {
            ChangeState(Player.State.Use);
        }
        // �Ʒ�Ű�� ������ ������ DuckState
        else if (player.MoveDir.y < -0.1f)
        {
            ChangeState(Player.State.Duck);
        }
        // JumpŰ�� ������ ������ JumpState
        else if (player.Input.actions["Jump"].IsPressed() && player.Input.actions["Jump"].triggered)
        {
            Jump();
            if (!player.IsGrounded)
            {
                ChangeState(Player.State.Jump);
            }
        }
        // �������� JumpState
        else if (!player.IsGrounded)
        {
            ChangeState(Player.State.Jump);
        }
        // ���� ��ٸ��� �ְ� ��Ű�� ������ ClimbState
        else if (player.IsLadder && player.MoveDir.y > 0f)
        {
            ChangeState(Player.State.Climb);
        }
        // ���Ű�� ������ GrabState
        else if (player.Input.actions["Grab"].IsPressed() && player.Input.actions["Grab"].triggered)
        {
            ChangeState(Player.State.Grab);
        }
    }

    public NormalState(Player player) : base(player) { }
}
