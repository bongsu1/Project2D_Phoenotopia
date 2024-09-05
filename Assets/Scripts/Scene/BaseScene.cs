using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseScene : MonoBehaviour
{
    [SerializeField] protected CharacterStatusRender statusRender;
    [SerializeField] protected Transform[] startPoint;
    [SerializeField] protected int exitPoint; // ������ ���� Ȯ���� ���� ������ ��� ������ ���ϱ� ���� ���� ����Ʈ ����
    [SerializeField] protected Vector2 battlePosition; // ���Ϳ� ����� ����Ǵ� ����Ʈ, ������� ��Ʋ������ ���
    [SerializeField] protected EscapeMenuUI escapeMenu;
    [SerializeField] protected PlayerInput input;
    [SerializeField] protected AudioClip[] audioClips;
    [SerializeField] protected float bgmVolume;
    [SerializeField] protected float sfxVolume;

    public int ExitPoint { get { return exitPoint; } set { exitPoint = value; } }
    public Vector2 BattlePosition { get { return battlePosition; } set { battlePosition = value; } }
    public PlayerInput Input => input;

    public abstract IEnumerator LoadingRoutine();

    protected virtual void Awake()
    {
        if (input == null)
        {
            input = GetComponent<PlayerInput>();
        }
        escapeMenu = Manager.Resource.Load<EscapeMenuUI>($"UI/{typeof(EscapeMenuUI).Name}");
    }

    protected virtual void Start()
    {
        SetVolume();
    }

    protected virtual void OnEscape(InputValue value)
    {
        if (value.isPressed)
        {
            Manager.UI.ShowPopUpUI(escapeMenu);
            input.actions.Disable();
        }
    }

    private void SetVolume()
    {
        Manager.Sound.PlayBGM(audioClips[0]);
        Manager.Sound.BGMVolme = bgmVolume;
        Manager.Sound.SFXVolme = sfxVolume;
    }
}
