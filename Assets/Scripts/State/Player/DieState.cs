using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class DieState : PlayerState
{
    public override void Enter()
    {
        player.Animator.Play("Knockdown");
    }

    public DieState(Player player)
    {
        this.player = player;
    }
}
