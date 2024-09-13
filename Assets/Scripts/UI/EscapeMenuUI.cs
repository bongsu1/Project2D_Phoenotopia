using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeMenuUI : PopUpUI
{
    [Header("Select Menu")]
    [SerializeField] Color normalColor;
    [SerializeField] Color selectColor;
    [SerializeField] float normalSize;
    [SerializeField] float selectSize;

    private BaseScene curScene;
    private TextMeshProUGUI[] script;
    private Player player;
    private WorldPlayer worldPlayer;

    private bool isTitle;
    private int selectIndex;
    private int PressButton;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        selectIndex = 1;
        if (curScene == null)
        {
            curScene = FindObjectOfType<BaseScene>();
        }

        if (curScene.name == "TitleScene")
            isTitle = true;
        else
            isTitle = false;

        script = GetComponentsInChildren<TextMeshProUGUI>();
        script[0].text = isTitle ? "Exit to Desktop" : "Quit to Title Screen";

        if (curScene.name == "WorldScene")
        {
            worldPlayer = FindObjectOfType<WorldPlayer>();
            worldPlayer.Input.actions.Disable();
        }
        else if (curScene.name != "TitleScene")
        {
            player = FindObjectOfType<Player>();
            player.Input.actions.Disable();
        }

        Choice();
    }

    private void Choice()
    {
        for (int i = 1; i < script.Length; i++)
        {
            if (selectIndex == i)
            {
                script[i].color = selectColor;
                script[i].fontSize = selectSize;
                continue;
            }
            script[i].color = normalColor;
            script[i].fontSize = normalSize;
        }
    }

    private void OnArrow(InputValue value)
    {
        PressButton = (int)value.Get<float>();
        selectIndex += PressButton;
        if (selectIndex >= script.Length)
        {
            selectIndex = 1;
        }
        else if (selectIndex == 0)
        {
            selectIndex = script.Length - 1;
        }
        Choice();
    }

    private void OnEscape(InputValue value)
    {
        Close();
    }

    private void OnX(InputValue value)
    {
        switch (selectIndex)
        {
            case 1:
                Close();
                break;
            case 2:
                if (isTitle)
                {
                    Application.Quit();
                    Close();
                }
                else
                {
                    Manager.Scene.LoadScene("TitleScene");
                    Close();
                }
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        curScene.Input.actions.Enable();

        if (curScene.name == "WorldScene")
        {
            worldPlayer.Input.actions.Enable();
        }
        else if (curScene.name != "TitleScene")
        {
            player.Input.actions.Enable();
        }
    }
}
