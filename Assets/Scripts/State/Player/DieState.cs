public class DieState : PlayerState
{
    public override void Enter()
    {
        player.Animator.Play("Knockdown");
    }

    public DieState(Player player) : base(player) { }
}
