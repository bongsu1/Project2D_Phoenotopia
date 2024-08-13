using UnityEngine;

public class PlayerState : BaseState<Player.State>
{
    protected Player player;

    protected virtual void Move()
    {
        // 목표속도, 바라보고 있는 방향으로의 최대속도
        float target = player.MoveDir.x * player.MoveSpeed;

        // 목표속도와 현재속도의 차이, 차이가 클수록 가속이 커진다
        float diffSpeed = target - player.Rigid.velocity.x;

        player.Rigid.AddForce(Vector2.right * diffSpeed * player.Accel);
    }

    protected virtual void Jump()
    {
        player.Rigid.velocity = new Vector2(player.Rigid.velocity.x, player.JumpSpeed);
    }

    public PlayerState(Player player)
    {
        this.player = player;
    }
}
