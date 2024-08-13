using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public enum State { Idle, Talk, Walk, Action }

    private State curState;
    [Header("NPC")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Transform[] checkPoint;
    [SerializeField] float idleTime;
    [SerializeField] float talkingDistance;

    [Header("TalkUI")]
    [SerializeField] Canvas speechUI;
    [SerializeField] Image speechBubble;
    [SerializeField] TMP_Text talkScript;
    [SerializeField] Vector3 offset;

    [Header("Talk Script")]
    [SerializeField] string[] scripts;

    private Player player;
    private Coroutine talkRoutine;
    private Coroutine idleRoutine;

    WaitUntil waitUntil;
    WaitForSeconds waitIdle;

    Transform dirPoint;
    private int index;
    private float direction;

    private void Start()
    {
        direction = 1;
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

        direction = Mathf.Sign(dirPoint.position.x - transform.position.x);
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
        StartCoroutine(PlayerMoveRoutine(player));
        Talk();
    }

    IEnumerator PlayerMoveRoutine(Player player)
    {
        float playerXPos = transform.position.x + (talkingDistance * direction);
        player.transform.localScale = new Vector2(direction, 1f);

        while (Mathf.Abs(playerXPos - player.transform.position.x) > 0.1f)
        {
            player.Rigid.velocity = Vector2.right * direction * 2f;
            yield return null;
        }

        player.Rigid.velocity = Vector2.zero;
        player.transform.localScale = new Vector2(-direction, 1f);
    }

    private void Talk()
    {
        StopIdleRoutine();
        talkRoutine = StartCoroutine(TalkRoutine());
    }

    IEnumerator TalkRoutine()
    {
        speechUI.gameObject.SetActive(true);
        curState = State.Talk;
        for (int i = 0; i < scripts.Length; i++)
        {
            //new WaitUntil(() => player.Input.actions["Attack"].IsPressed() && player.Input.actions["Attack"].triggered);
            yield return waitUntil;
            talkScript.text = scripts[i];
            yield return new WaitForSeconds(.1f);
        }
        yield return waitUntil;

        talkRoutine = null;
        player.OnTalk = false;
        player = null;
        waitUntil = null;
        curState = State.Idle;
        speechUI.gameObject.SetActive(false);
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

    private void LateUpdate()
    {
        if (curState == State.Talk)
        {
            speechBubble.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);
        }
    }
}
