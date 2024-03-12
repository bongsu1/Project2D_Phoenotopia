using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBPatrolState : BoarState
{
    Transform dirPoint;
    int index = 0;
    float direction;

    public override void Enter()
    {
        boar.Animator.Play("Walk");
        dirPoint = boar.PatrolPoint[index];
    }

    public override void Update()
    {
        direction = Mathf.Sign(dirPoint.position.x - boar.transform.position.x);
        boar.transform.localScale = new Vector3(direction, 1f, 1f);

        boar.PlayerCheck();
    }

    public override void FixedUpdate()
    {
        boar.Rigid.velocity = new Vector2(direction * boar.WalkSpeed, boar.Rigid.velocity.y);
    }

    public override void Exit()
    {
        index++;
        if (index == boar.PatrolPoint.Length)
        {
            index = 0;
        }
    }

    public override void Transition()
    {
        if (boar.OnPlayerCheck)
        {
            ChangeState(Boar.State.Attack);
        }
        else if (Mathf.Abs(dirPoint.position.x - boar.transform.position.x) < 0.01f)
        {
            ChangeState(Boar.State.Idle);
        }
    }

    public EBPatrolState(Boar boar)
    {
        this.boar = boar;
    }
}
