using UnityEngine;

public class DuckState : PlayerState
{
    public override void Enter()
    {
        player.MoveSpeed = player.DuckMoveSpeed;

        if (Mathf.Abs(player.Rigid.velocity.x) > 2.5f)
        {
            player.Animator.Play("Roll");
            player.SFX.PlaySFX(PlayerSoundManager.SFX.Roll);
        }
        else
        {
            player.Animator.Play("Duck");
        }
        player.PlayerColl.offset = new Vector2(0f, 0.25f);
        player.PlayerColl.size = new Vector2(0.45f, 0.5f);
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
        player.Animator.speed = 1f;
    }

    protected override void Move()
    {
        base.Move();

        // Run 키를 누를 때 대시(구르기) 추가예정
        if (Mathf.Abs(player.Rigid.velocity.x) > 2.1f)
        {
            player.Animator.Play("Roll");
        }
        else if (Mathf.Abs(player.MoveDir.x) > 0)
        {
            player.Animator.Play("DuckMove"); // +움직이고 있지 않을 때 애니메이션 정지
        }

        if (Mathf.Abs(player.Rigid.velocity.x) < 0.01f && player.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Duck")
        {
            player.Animator.speed = 0f;
        }
        else
        {
            player.Animator.speed = 1f;
        }
    }

    public override void Transition()
    {
        if (player.OnCeiling)
            return;

        if (!player.IsDucking)
        {
            ChangeState(Player.State.Normal);
        }
        else if (!player.IsGrounded)
        {
            ChangeState(Player.State.Jump);
        }
    }

    public DuckState(Player player) : base(player) { }
}
