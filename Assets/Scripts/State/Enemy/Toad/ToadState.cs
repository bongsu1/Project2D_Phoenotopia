public class ToadState : BaseState<Toad.State>
{
    protected Toad toad;
    
    public ToadState(Toad toad)
    {
        this.toad = toad;
    }
}
