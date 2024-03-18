using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DieScene : BaseScene
{
    [SerializeField] Player Player;

    public UnityEvent OnPressXButton;

    private int count = 1;

    public override IEnumerator LoadingRoutine()
    {
        count = 1;
        exitPoint = 0;
        yield return null;
        Player.EndGame();
        yield return null;
    }

    private void TitleSceneLoad()
    {
        Manager.Scene.LoadScene("TitleScene");
    }

    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            TitleSceneLoad();
            OnPressXButton?.Invoke();
            count--;
        }
    }
}
