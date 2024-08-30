using Unity.VisualScripting;
using UnityEngine;

public class GrabState : PlayerState
{
    float facing; // 바라보는 방향을 저장
    Vector2 offset;

    public override void Enter()
    {
        player.MoveSpeed = player.GrabMoveSpeed;

        player.Grab();
        if (player.Box != null)
        {
            player.Animator.Play("Push");
            player.Animator.speed = 0;

            facing = player.transform.localScale.x;

            float xSize = player.Box.BoxColl.size.x;
            float ySize = player.Box.BoxColl.size.y;

            offset = new Vector2(Mathf.Abs(xSize + player.PlayerColl.size.x) * 0.5f, (ySize * 0.5f) + 0.0078125f);
            player.Box.transform.localPosition = offset;

            player.Box.Rigid.bodyType = RigidbodyType2D.Kinematic;
            player.Box.Rigid.useFullKinematicContacts = true;
            Physics2D.IgnoreCollision(player.Box.BoxColl, player.PlayerColl);
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
            if (player.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("Pull"))
                player.Animator.Play("Pull");
            else
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

        if (player.Box == null)
            return;

        player.Box.Rigid.bodyType = RigidbodyType2D.Dynamic;
        Physics2D.IgnoreCollision(player.Box.BoxColl, player.PlayerColl, false);
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

    protected override void Move()
    {
        base.Move();

        if (player.Box == null)
            return;

        //                                                                                       0.02f = fixedDeltaTime
        player.Box.Rigid.MovePosition(player.Box.transform.position + Vector3.right * player.Rigid.velocity.x * 0.02f);
    }

    public GrabState(Player player) : base(player) { }
}
