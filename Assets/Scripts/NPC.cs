using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public enum State { Idle, Talk, Walk, Action }

    private State state;

    [SerializeField] Animator animator;

    private Player player;
    private Coroutine talkRoutine;
    WaitUntil waitUntil;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                IdleUpdate();
                break;
            case State.Talk:
                TalkUpdate();
                break;
            case State.Walk:
                break;
            case State.Action:
                break;
        }
    }

    private void IdleUpdate()
    {
        animator.Play("Idle");
    }
    
    private void TalkUpdate()
    {
        animator.Play("Talk");
        if (player.Input.actions["Jump"].IsPressed() && player.Input.actions["Jump"].triggered)
        {
            StopTalkRoutine();
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
        talkRoutine = StartCoroutine(TalkRoutine());
    }

    IEnumerator TalkRoutine()
    {
        // test
        state = State.Talk;
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
        state = State.Idle;
    }

    private void StopTalkRoutine()
    {
        StopCoroutine(talkRoutine);
        talkRoutine = null;
        player.OnTalk = false;
        player = null;
        state = State.Idle;
    }
}
