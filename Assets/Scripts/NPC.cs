using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public enum State { Idle, Talk, Walk, Action }

    private State curState;

    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Transform[] checkPoint;
    [SerializeField] float idleTime;

    private Player player;
    private Coroutine talkRoutine;
    private Coroutine idleRoutine;

    WaitUntil waitUntil;
    WaitForSeconds waitIdle;

    Transform dirPoint;
    private int index;

    private void Start()
    {
        index = 0;
        waitIdle = new WaitForSeconds(idleTime);
        dirPoint = checkPoint.Length > 1 ? checkPoint[index] : null;
        curState = State.Idle;
    }

    private void Update()
    {
        switch (curState)
        {
            case State.Idle:
                IdleUpdate();
                break;
            case State.Talk:
                TalkUpdate();
                break;
            case State.Walk:
                WalkUpdate();
                break;
            case State.Action:
                break;
        }
    }

    private void IdleUpdate()
    {
        animator.Play("Idle");

        // 움직이지 않는 NPC는 계속 Idle 상태
        if (checkPoint.Length > 1 && idleRoutine == null)
        {
            idleRoutine = StartCoroutine(IdleRoutine());
        }
    }

    private void TalkUpdate()
    {
        animator.Play("Talk");
    }

    private void WalkUpdate()
    {
        animator.Play("Walk");

        float direction = Mathf.Sign(dirPoint.position.x - transform.position.x);
        transform.localScale = new Vector3(direction, 1f, 1f);

        rigid.velocity = new Vector2(direction, rigid.velocity.y);

        if (dirPoint == checkPoint[index] && Mathf.Abs(checkPoint[index].position.x - transform.position.x) < 0.1f)
        {
            index++;
            if (index == checkPoint.Length)
            {
                index = 0;
            }
            dirPoint = checkPoint[index];
            curState = State.Idle;
        }
    }

    public void Interact(Player player)
    {
        if (talkRoutine != null)
            return;

        this.player = player;
        waitUntil = new WaitUntil(() => player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered);
        Talk();
    }

    private void Talk()
    {
        StopIdleRoutine();
        talkRoutine = StartCoroutine(TalkRoutine());
    }

    IEnumerator TalkRoutine()
    {
        // test player talkState 수정바람
        curState = State.Talk;
        yield return waitUntil;
        Debug.Log("Talk 1");
        yield return new WaitForSeconds(.1f);
        yield return waitUntil;
        Debug.Log("Talk 2");
        yield return new WaitForSeconds(.1f);
        yield return waitUntil;
        Debug.Log("Talk 3");
        yield return new WaitForSeconds(.1f);
        yield return waitUntil;
        Debug.Log("Talk 4");
        yield return new WaitForSeconds(.1f);
        yield return waitUntil;
        talkRoutine = null;
        player.OnTalk = false;
        player = null;
        curState = State.Idle;
    }

    IEnumerator IdleRoutine()
    {
        yield return waitIdle;
        curState = State.Walk;
        idleRoutine = null;
    }

    private void StopIdleRoutine()
    {
        if (idleRoutine != null)
        {
            StopCoroutine(idleRoutine);
            idleRoutine = null;
        }
    }
}
