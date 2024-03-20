public class ETIdleState : ToadState
{
    public override void Enter()
    {
        toad.Animator.Play("Idle");
    }

    public override void Update()
    {
        toad.PlayerCheck();
    }

    public override void Transition()
    {
        if (toad.OnPlayerCheck)
        {
            ChangeState(Toad.State.Attack);
        }
    }

    public ETIdleState(Toad toad)
    {
        this.toad = toad;
    }
}
