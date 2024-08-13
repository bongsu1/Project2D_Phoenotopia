using UnityEngine;

public class ETDieState : ToadState
{
    public override void Enter()
    {
        toad.Rigid.constraints = RigidbodyConstraints2D.None;
        toad.Animator.Play("Die");
        toad.DestroyGameObject();
        // ������ ������
    }

    public ETDieState(Toad toad) : base(toad) { }
}
