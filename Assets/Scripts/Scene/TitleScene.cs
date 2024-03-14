using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TitleScene : BaseScene
{
    public UnityEvent OnXButtonPress;

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }

    // 게임씬으로 전환
    private void GameStart()
    {
        Manager.Scene.LoadScene("TownScene");
    }

    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            OnXButtonPress?.Invoke();
            GameStart();
        }
    }
}
