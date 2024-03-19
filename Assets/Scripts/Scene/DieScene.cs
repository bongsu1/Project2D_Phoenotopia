using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DieScene : BaseScene
{
    [SerializeField] Player Player;

    public UnityEvent OnPressXButton;

    private int count = 1;

    protected override void Start()
    {
        Manager.Sound.PlaySFX(audioClips[0]);
        Manager.Sound.BGMVolme = bgmVolume;
        Manager.Sound.SFXVolme = sfxVolume;
    }

    public override IEnumerator LoadingRoutine()
    {
        count = 1;
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
            Manager.Sound.PlaySFX(audioClips[1]);
        }
    }
}
