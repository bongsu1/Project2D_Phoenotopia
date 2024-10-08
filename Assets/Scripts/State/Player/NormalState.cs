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
        // 공격키를 누르면 AttackState
        if (Manager.Data.Stamina >= player.UseStamina && !player.OnNPC && player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered)
        {
            Manager.Data.StartStaminaRegenRoutine();
            ChangeState(Player.State.Attack);
        }
        // 스태미나가 부족한 상태에서 공격키를 누르고 있으면 ChargeState
        else if (Manager.Data.Stamina < player.UseStamina && !player.OnNPC && player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered)
        {
            ChangeState(Player.State.Charge);
        }
        // NPC앞에서 공격키를 누르면 TalkState
        else if (player.OnNPC && player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered)
        {
            ChangeState(Player.State.Talk);
        }
        // Use키를 누르면 UseState
        else if (Manager.Data.Stamina >= player.UseStamina && player.Input.actions["Use"].IsPressed() && player.Input.actions["Use"].triggered)// (+아이템을 장착하고 있으면, 아이템이 구현되었을때 추가)
        {
            ChangeState(Player.State.Use);
        }
        // 아래키를 누르고 있으면 DuckState
        else if (player.MoveDir.y < -0.1f)
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
        // 잡기키를 누르면 GrabState
        else if (player.Input.actions["Grab"].IsPressed() && player.Input.actions["Grab"].triggered)
        {
            ChangeState(Player.State.Grab);
        }
    }

    public NormalState(Player player) : base(player) { }
}
