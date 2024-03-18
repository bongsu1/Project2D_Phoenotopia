using System.Collections;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    [SerializeField] protected CharacterStatusRender statusRender;
    [SerializeField] protected Transform[] startPoint;
    [SerializeField] protected int exitPoint; // 나가는 지점 확인후 다음 씬에서 어디서 나올지 정하기 위해 나간 포인트 저장
    [SerializeField] protected Vector2 battlePosition; // 몬스터와 조우시 저장되는 포인트, 월드씬과 배틀씬에서 사용

    public int ExitPoint { get { return exitPoint; } set { exitPoint = value; } }
    public Vector2 BattlePosition { get { return battlePosition; } set { battlePosition = value; } }

    public abstract IEnumerator LoadingRoutine();
}
