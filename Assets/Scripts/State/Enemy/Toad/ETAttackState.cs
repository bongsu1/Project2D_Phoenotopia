using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETAttackState : ToadState
{
    public override void Enter()
    {
        toad.StartAttackRoutine();
        toad.AttackCount--;
    }

    public override void Update()
    {
        toad.Animator.SetFloat("Fall", toad.Rigid.velocity.y);

        float dirction = Mathf.Sign(toad.Player.transform.position.x - toad.transform.position.x);
        toad.transform.localScale = new Vector3(dirction, 1f, 1f);
    }

    public override void Exit()
    {
        toad.StopAttackRoutine();
    }

    public ETAttackState(Toad toad)
    {
        this.toad = toad;
    }
}
