using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
    public UnityEvent OnXButtonPress;
    [SerializeField] Image textImage;

    private int count = 1;

    public override IEnumerator LoadingRoutine()
    {
        count = 1;
        Manager.Data.Hp = 30;
        yield return null;
    }

    // 게임씬으로 전환
    private void GameStart()
    {
        Manager.Scene.LoadScene("TownScene");
        textImage.gameObject.SetActive(false);
    }

    private void OnAttack(InputValue value)
    {
        if (value.isPressed && count > 0)
        {
            OnXButtonPress?.Invoke();
            GameStart();
            count--;
        }
    }
}
