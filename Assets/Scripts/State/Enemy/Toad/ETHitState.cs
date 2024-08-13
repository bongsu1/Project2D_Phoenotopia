public class ETHitState : ToadState
{
    public override void Enter()
    {
        toad.Animator.Play("Hit");
    }

    public override void Update()
    {
        toad.Animator.SetFloat("Fall", toad.Rigid.velocity.y);
    }

    public ETHitState(Toad toad) : base(toad) { }
}
