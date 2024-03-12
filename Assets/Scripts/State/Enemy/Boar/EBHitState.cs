using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBHitState : BoarState
{
    public override void Enter()
    {
        boar.Animator.Play("Hit");
    }

    public override void Update()
    {
        boar.Animator.SetFloat("Fall", boar.Rigid.velocity.y);
    }

    public EBHitState(Boar boar)
    {
        this.boar = boar;
    }
}
