using System.Collections;
using UnityEngine;

public class TalkState : PlayerState
{
    private float zoomIn = 180;
    private float zoomOut = 140;

    public override void Enter()
    {
        Manager.UI.CloseTutorialUI();

        player.Talk();
        player.Input.actions["Move"].Disable();

        player.Animator.Play("Idle");
        player.StartCoroutine(ZoomInRoutine());
    }

    public override void Update()
    {
        player.Animator.SetFloat("MoveSpeed", Mathf.Abs(player.Rigid.velocity.x));
    }

    public override void Exit()
    {
        player.StartCoroutine(ZoomOutRoutine());
        player.Input.actions["Move"].Enable();
    }

    public override void Transition()
    {
        if (!player.OnTalk)
        {
            ChangeState(Player.State.Normal);
        }
    }

    IEnumerator ZoomInRoutine()
    {
        float rate = 0;
        while (rate < 1)
        {
            rate += Time.deltaTime;
            player.Pixel.assetsPPU = (int)Mathf.Lerp(zoomOut, zoomIn, rate);
            yield return null;
        }

        player.Pixel.assetsPPU = 180;
    }

    IEnumerator ZoomOutRoutine()
    {
        float rate = 0;
        while (rate < 1)
        {
            rate += Time.deltaTime;
            player.Pixel.assetsPPU = (int)Mathf.Lerp(zoomIn, zoomOut, rate);
            yield return null;
        }

        player.Pixel.assetsPPU = 140;
    }

    public TalkState(Player player) : base(player) { }
}
