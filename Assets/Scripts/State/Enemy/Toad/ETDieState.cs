using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ETDieState : ToadState
{
    public override void Enter()
    {
        toad.Rigid.constraints = RigidbodyConstraints2D.None;
        toad.Animator.Play("Die");
        toad.DestroyGameObject();
        // 아이템 떨구기
    }

    public ETDieState(Toad toad)
    {
        this.toad = toad;
    }
}
