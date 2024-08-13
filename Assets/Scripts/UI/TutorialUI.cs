using TMPro;
using UnityEngine;

public class TutorialUI : BaseUI
{
    [Header("UI")]
    [SerializeField] TMP_Text image;
    [SerializeField] TMP_Text descript;

    [Header("Sprite & Descript")]
    [SerializeField] string[] buttonSprites;
    [SerializeField] string[] descripts;
    [SerializeField] TutorialType type;

    [Header("Offset")]
    [SerializeField] Vector3 offset;

    [SerializeField] private Transform followTarget;
    public Transform FollowTarget { get { return followTarget; } set { followTarget = value; } }


    public void SetTutorial(TutorialType type)
    {
        image.text = buttonSprites[(int)type];
        descript.text = $": {descripts[(int)type]}";
    }

    private void LateUpdate()
    {
        if (followTarget == null)
            return;

        transform.position = Camera.main.WorldToScreenPoint(followTarget.position) + offset;
    }
}

public enum TutorialType { WakeUp, Talk, Enter, Climb } // 그외에는 더 필요할 때 만들예정