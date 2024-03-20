using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
    public UnityEvent OnXButtonPress;
    [SerializeField] Image textImage;

    private int count;

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }

    protected override void Start()
    {
        exitPoint = 0;
        count = 1;
        Manager.Data.Hp = Manager.Data.MaxHp;
        base.Start();
    }

    // 게임씬으로 전환
    private void GameStart()
    {
        Manager.Scene.LoadScene("TownScene");
        textImage.gameObject.SetActive(false);
    }

    private void OnX(InputValue value)
    {
        if (value.isPressed && count > 0)
        {
            OnXButtonPress?.Invoke();
            GameStart();
            count--;
            Manager.Sound.StopBGM();
            Manager.Sound.PlaySFX(audioClips[1]);
            Manager.Sound.FadeOutSFX();
        }
    }
}
