public class SleepState : PlayerState
{
    public override void Enter()
    {
        Manager.UI.ShowTutorialUI(player.transform, TutorialType.WakeUp);
        player.Animator.Play("Sleep");
    }

    public override void Update()
    {
        // 코루틴 대신 애니메이션에서 ChangeState를 호출
        if (player.Input.actions["Jump"].IsPressed() && player.Input.actions["Jump"].triggered)
        {
            Manager.UI.CloseTutorialUI();
            player.Animator.Play("WakeUp");
            player.SFX.PlaySFX(PlayerSoundManager.SFX.WakeUp);
        }
    }

    public SleepState(Player player) : base(player) { }
}
