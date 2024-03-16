using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : PlayerState
{
    public override void Enter()
    {
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

    public HitState(Player player)
    {
        this.player = player;
    }
}
