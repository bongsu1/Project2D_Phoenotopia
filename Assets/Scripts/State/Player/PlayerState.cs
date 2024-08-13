using UnityEngine;

public class PlayerState : BaseState<Player.State>
{
    protected Player player;

    protected virtual void Move()
    {
        // ��ǥ�ӵ�, �ٶ󺸰� �ִ� ���������� �ִ�ӵ�
        float target = player.MoveDir.x * player.MoveSpeed;

        // ��ǥ�ӵ��� ����ӵ��� ����, ���̰� Ŭ���� ������ Ŀ����
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
