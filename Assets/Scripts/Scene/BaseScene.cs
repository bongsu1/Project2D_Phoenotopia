using System.Collections;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    [SerializeField] protected Transform[] startPoint;
    [SerializeField] protected int exitPoint; // 나가는 지점 확인후 다음 씬에서 어디서 나올지 정하기 위해 나간 포인트 저장

    public int ExitPoint { get { return exitPoint; } set { exitPoint = value; } }

    public abstract IEnumerator LoadingRoutine();
}
