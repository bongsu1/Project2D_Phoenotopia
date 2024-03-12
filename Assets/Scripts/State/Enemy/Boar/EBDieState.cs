using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBDieState : BoarState
{
    public override void Enter()
    {
        boar.Animator.Play("Die");
        boar.DestroyGameObject();
        // 아이템 떨구기
    }

    public EBDieState(Boar boar)
    {
        this.boar = boar;
    }
}
