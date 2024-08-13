public class SleepState : PlayerState
{
    public override void Enter()
    {
        Manager.UI.ShowTutorialUI(player.transform, TutorialType.WakeUp);
        player.Animator.Play("Sleep");
    }

    public override void Update()
    {
        // �ڷ�ƾ ��� �ִϸ��̼ǿ��� ChangeState�� ȣ��
        if (player.Input.actions["Jump"].IsPressed() && player.Input.actions["Jump"].triggered)
        {
            Manager.UI.CloseTutorialUI();
            player.Animator.Play("WakeUp");
            player.SFX.PlaySFX(PlayerSoundManager.SFX.WakeUp);
        }
    }

    public SleepState(Player player) : base(player) { }
}
