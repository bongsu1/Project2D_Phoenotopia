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

    public override IEnumerator LoadingRoutine()
    {
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
        if (value.isPressed)
        {
            OnXButtonPress?.Invoke();
            GameStart();
        }
    }
}
