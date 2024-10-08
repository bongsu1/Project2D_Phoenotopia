using UnityEngine;
using UnityEngine.Events;

public class HitSwitch : MonoBehaviour, IDamagable
{
    [SerializeField] Animator animator;

    public UnityEvent OnSwitch;
    public UnityEvent OffSwitch;

    private bool isPlay;       // 연결되어 있는 오브젝트의 동작상태
    private bool onSwitching;  // 스위치의 동작 상태(애니메이션 동작)

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

    public void Knockback(Vector2 hitPoint, float hitPower) { } // 넉백하지 않음
}
