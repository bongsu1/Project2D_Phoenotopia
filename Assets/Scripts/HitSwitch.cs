using UnityEngine;
using UnityEngine.Events;

public class HitSwitch : MonoBehaviour, IDamagable
{
    [SerializeField] Animator animator;

    public UnityEvent OnSwitch;
    public UnityEvent OffSwitch;

    private bool isPlay;       // ����Ǿ� �ִ� ������Ʈ�� ���ۻ���
    private bool onSwitching;  // ����ġ�� ���� ����(�ִϸ��̼� ����)

    public void TakeDamage(int damage)
    {
        if (isPlay || onSwitching)
            return;

        onSwitching = true;
        animator.SetTrigger("Hit");
    }

    public void FinishSwitchingAnimation()
    {
        onSwitching = false;
    }

    public void SwitchOn()
    {
        OnSwitch?.Invoke();
    }

    public void SwitchOff()
    {
        OffSwitch?.Invoke();
    }

    public void IsPlaySet(bool value)
    {
        isPlay = value;
    }

    public void Knockback(Vector2 hitPoint, float hitPower) { } // �˹����� ����
}
