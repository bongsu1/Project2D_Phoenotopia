using System.Collections;
using UnityEngine;

public class UseState : PlayerState
{
    // 장착한 아이템 종류에 따라 다르게 업데이트 (무기, 음식)
    // 임시로 새총만 구현
    Transform aim;
    Quaternion bottomTurnPoint;
    Quaternion topTurnPoint;
    float rotateDir;

    Coroutine aimFlashRoutine;
    SpriteRenderer renderer;

    public override void Enter()
    {
        bottomTurnPoint = Quaternion.Euler(0f, 0f, -40f);
        topTurnPoint = Quaternion.Euler(0f, 0f, 50f);

        player.Animator.Play("SlingshotStart");
        player.Rigid.velocity = Vector3.zero;

        aim = player.SlingshotAim;
        aim.localRotation = Quaternion.Euler(0f, 0f, -39f);
        rotateDir = -0.1f;
        aim.gameObject.SetActive(true);

        renderer = aim.GetComponentInChildren<SpriteRenderer>();
        aimFlashRoutine = player.StartCoroutine(AimFlashRoutine());
    }

    public override void Update()
    {
        if (aim.localRotation.z < bottomTurnPoint.z)
        {
            player.Animator.Play("AimUp");
            rotateDir = 1f;
        }
        else if (aim.localRotation.z > topTurnPoint.z)
        {
            player.Animator.Play("AimDown");
            rotateDir = -1f;
        }
        aim.Rotate(0, 0, player.AimSpeed * rotateDir * Time.deltaTime);

        if (player.Input.actions["Use"].IsPressed() && player.Input.actions["Use"].triggered)
        {
            rotateDir = 0f;
            player.Animator.Play("SlingshotEnd");
            player.Shot();
            player.SFX.PlaySFX(PlayerSoundManager.SFX.Shot);

            Manager.Data.Stamina -= player.UseStamina;
            Manager.Data.StartStaminaRegenRoutine();
        }
    }

    public override void Exit()
    {
        aim.gameObject.SetActive(false);
        player.StopCoroutine(AimFlashRoutine());
    }

    public override void Transition()
    {
        if (player.Input.actions["Jump"].IsPressed() && player.Input.actions["Jump"].triggered)
        {
            ChangeState(Player.State.Normal);
        }
    }

    IEnumerator AimFlashRoutine()
    {
        while (true)
        {
            renderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            renderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public UseState(Player player)
    {
        this.player = player;
    }
}
