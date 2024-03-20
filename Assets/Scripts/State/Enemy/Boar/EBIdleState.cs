using UnityEngine;

public class EBIdleState : BoarState
{
    public override void Enter()
    {
        boar.Rigid.velocity = Vector2.zero;
        boar.Animator.Play("Idle");
    }

    public override void Update()
    {
        boar.PlayerCheck();
    }

    public override void Transition()
    {
        if (boar.OnPlayerCheck)
        {
            ChangeState(Boar.State.Attack);
        }
    }

    public EBIdleState(Boar boar)
    {
        this.boar = boar;
    }
}
