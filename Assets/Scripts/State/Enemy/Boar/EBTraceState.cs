using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBTraceState : BoarState
{
    float direction;
    float time;

    public override void Enter()
    {
        time = 0;
        boar.Rigid.velocity = Vector3.zero;
    }

    public override void Update()
    {
        direction = Mathf.Sign(boar.Player.transform.position.x - boar.transform.position.x);

        boar.Animator.Play("Walk");
        boar.transform.localScale = new Vector3(direction, 1f, 1f);

        time += Time.deltaTime;
    }

    public override void FixedUpdate()
    {
        if (Vector2.Distance(boar.transform.position, boar.Player.transform.position) < boar.CheckSize)
        {
            boar.Rigid.velocity = new Vector2(direction * boar.WalkSpeed * 0.5f, boar.Rigid.velocity.y);
        }
        else
        {
            boar.Rigid.velocity = new Vector2(direction * boar.WalkSpeed, boar.Rigid.velocity.y);
        }
    }

    public override void Exit()
    {
        if (time >= boar.AttackCool)
        {
            boar.AttackCount = 1;
        }
    }

    public override void Transition()
    {
        // 공격 쿨타임이 다 되고 거리가 가까워 지면 AttackState
        if ((Vector2.Distance(boar.transform.position, boar.Player.transform.position) < boar.CheckSize)
            && (time >= boar.AttackCool))
        {
            ChangeState(Boar.State.Attack);
        }
    }

    public EBTraceState(Boar boar)
    {
        this.boar = boar;
    }
}
