using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    public enum State { Idle, Patrol, Attack, Trace, Hit, Die }
    private StateMachine<State> stateMachine = new StateMachine<State>();

    [SerializeField] Transform[] patrolPoint;

    [Header("Attack")]
    [SerializeField] float runHitPower;
    [SerializeField] float jumpHitPower;

    [Header("Move")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float maxjumpDistance;
    [SerializeField] float jumpMultiplier;

    [Header("Time")]
    [SerializeField] float holdTime;
    [SerializeField] float attackCool;
    [SerializeField] float runTime;
    [SerializeField] float dieTime;

    private WaitForSeconds holdWait;
    private WaitForSeconds attackWait;
    private Coroutine runAttackRoutine;
    private Coroutine jumpAttackRoutine;
    private bool onHit;
    private bool onJumpAttack;
    private bool onRunAttack;

    // property
    public Transform[] PatrolPoint => patrolPoint;
    public float WalkSpeed => walkSpeed;
    public float AttackCool => attackCool;

    private void Start()
    {
        for (int i = 0; i < patrolPoint.Length; i++)
        {
            float randomPoint = Random.Range(2f, 5f) * (i % 2 == 0 ? 1 : -1);
            patrolPoint[i].localPosition = new Vector2(randomPoint, 0);
        }

        holdWait = new WaitForSeconds(holdTime);
        attackWait = new WaitForSeconds(attackCool);
        playerCheck.radius = checkSize;

        stateMachine.AddState(State.Idle, new EBIdleState(this));
        stateMachine.AddState(State.Patrol, new EBPatrolState(this));
        stateMachine.AddState(State.Attack, new EBAttackState(this));
        stateMachine.AddState(State.Trace, new EBTraceState(this));
        stateMachine.AddState(State.Hit, new EBHitState(this));
        stateMachine.AddState(State.Die, new EBDieState(this));

        stateMachine.Start(State.Idle);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public override void TakeDamage(int damage)
    {
        if (onHit)
            return;

        base.TakeDamage(damage);
    }

    public override void Knockback(Vector2 hitPoint, float hitPower)
    {
        if (onHit)
            return;

        base.Knockback(hitPoint, hitPower);

        if (!onJumpAttack && (hp > 0))
        {
            onHit = true;
            stateMachine.ChangeState(State.Hit);
        }
        else if (hp <= 0)
        {
            stateMachine.ChangeState(State.Die);
        }
    }

    public void ToIdleState()
    {
        stateMachine.ChangeState(State.Idle);
    }

    public void ToPatrolState()
    {
        stateMachine.ChangeState(State.Patrol);
    }

    public void ToTraceState()
    {
        stateMachine.ChangeState(State.Trace);
    }

    // 돌진 공격 루틴
    public void StartRunAttackRoutine(float direction)
    {
        if (runAttackRoutine != null)
            return;

        runAttackRoutine = StartCoroutine(RunAttackRoutine(direction));
    }

    IEnumerator RunAttackRoutine(float direction)
    {
        yield return AttackCount > 0 ? null : attackWait;
        float time = 0;
        animator.Play("RunHold");
        yield return holdWait;

        onRunAttack = true;
        while (time < runTime)
        {
            time += Time.deltaTime;
            RunAttack(direction);
            yield return null;
        }
        onRunAttack = false;

        stateMachine.ChangeState(State.Trace);
    }

    public void StopRunAttackRoutine()
    {
        StopCoroutine(runAttackRoutine);
        runAttackRoutine = null;
    }

    // 돌진 공격
    private void RunAttack(float direction)
    {
        animator.Play("RunAttack");
        if (Mathf.Abs(rigid.velocity.x) < maxSpeed)
        {
            rigid.AddForce(Vector2.right * direction * runSpeed);
        }
    }

    public void StartJumpAttackRoutine(Vector2 playerPos)
    {
        if (jumpAttackRoutine != null)
            return;

        jumpAttackRoutine = StartCoroutine(JumpAttackRoutine(playerPos));
    }

    IEnumerator JumpAttackRoutine(Vector2 playerPos)
    {
        yield return AttackCount > 0 ? null : attackWait;
        animator.Play("JumpHold");
        yield return holdWait;
        JumpAttack(playerPos);
    }

    public void StopJumpAttackRoutine()
    {
        StopCoroutine(jumpAttackRoutine);
        jumpAttackRoutine = null;
    }

    private void JumpAttack(Vector2 playerPos)
    {
        float distance = playerPos.x - transform.position.x > maxjumpDistance ? maxjumpDistance : playerPos.x - transform.position.x;
        distance = Mathf.Abs(distance) > maxjumpDistance ? maxjumpDistance * Mathf.Sign(distance) : distance;
        rigid.velocity = new Vector2(distance * jumpMultiplier, jumpPower);
        animator.Play("JumpAttack");
        onJumpAttack = true;
    }

    public void Landing()
    {
        rigid.velocity = Vector2.zero;
        onHit = false;
        onJumpAttack = false;
    }

    public void DestroyGameObject()
    {
        Destroy(transform.parent.gameObject, dieTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onRunAttack && (((1 << collision.gameObject.layer) & PlayerLayer) != 0))
        {
            player.TakeDamage(damage);
            player.Knockback(transform.position, runHitPower);
        }
        else if (onJumpAttack && (((1 << collision.gameObject.layer) & PlayerLayer) != 0))
        {
            player.TakeDamage(damage);
            player.Knockback(transform.position, jumpHitPower);
        }
    }
}
