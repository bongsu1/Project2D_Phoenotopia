using UnityEngine;

public class HitState : PlayerState
{
    public override void Enter()
    {
        player.SFX.PlaySFX((PlayerSoundManager.SFX)(Random.Range(0, 2) + (int)PlayerSoundManager.SFX.Hurt));
        if (player.TakeHitPower > 9)
        {
            player.PlayerMaterial.bounciness = player.Bounciness;
            player.Rigid.sharedMaterial = player.PlayerMaterial;
            player.Animator.Play("Knockback");
            player.StartKnockbackRoutine(player.TakeHitPower);
        }
        else
        {
            player.Rigid.gravityScale = 0f;
            player.Animator.Play("Hit");
        }
    }

    public override void Exit()
    {
        player.PlayerMaterial.bounciness = 0f;
        player.Rigid.sharedMaterial = player.PlayerMaterial;
        player.Rigid.gravityScale = 1f;
        player.TakeHitPower = 0f;
        player.OnHit = false;
    }

    public HitState(Player player) : base(player) { }
}
