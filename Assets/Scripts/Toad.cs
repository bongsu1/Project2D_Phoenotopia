using System.Collections;
using UnityEngine;

public class Toad : Enemy // 개구리 몬스터
{
    public enum State { Idle, Attack, Hit, Die }
    protected StateMachine<State> stateMachine = new StateMachine<State>();

    [Header("Attack")]
    [SerializeField] float hitPower;

    [Header("Jump")]
    [SerializeField] float jumpPower;
    [SerializeField] float maxjumpDistance;
    [SerializeField] float jumpMultiplier;
    [SerializeField] float dieTime;

    [Header("Time")]
    [SerializeField] float attackCool;
    [SerializeField] float holdTime;

    private Coroutine attackRoutine;
    private WaitForSeconds attackWait;
    private WaitForSeconds holdWait;
    private bool onAttack;
    private bool onHit;

    private void Start()
    {
        attackWait = new WaitForSeconds(attackCool);
        holdWait = new WaitForSeconds(holdTime);
        playerCheck.radius = checkSize;

        stateMachine.AddState(State.Idle, new ETIdleState(this));
        stateMachine.AddState(State.Attack, new ETAttackState(this));
        stateMachine.AddState(State.Hit, new ETHitState(this));
        stateMachine.AddState(State.Die, new ETDieState(this));

        stateMachine.Start(State.Idle);
    }

    private void Update()
    {
        stateMachine.Update();
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

        if (!onAttack && (hp > 0))
        {
            onHit = true;
            stateMachine.ChangeState(State.Hit);
        }
        else if (hp <= 0)
        {
            stateMachine.ChangeState(State.Die);
        }
    }

    public void StartAttackRoutine()
    {
        if (attackRoutine != null)
            return;

        attackRoutine = StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return AttackCount > 0 ? null : attackWait;
            animator.Play("Hold");
            yield return holdWait;
            Jump(player.transform.position);
        }
    }

    public void StopAttackRoutine()
    {
        StopCoroutine(attackRoutine);
        attackRoutine = null;
    }

    // 플레이어 방향으로 점프
    private void Jump(Vector2 playerPos)
    {
        float distance = playerPos.x - transform.position.x > maxjumpDistance ? maxjumpDistance : playerPos.x - transform.position.x;
        distance = Mathf.Abs(distance) > maxjumpDistance ? maxjumpDistance * Mathf.Sign(distance) : distance;
        float amount = (maxjumpDistance - Mathf.Abs(distance)) / 2;
        rigid.velocity = new Vector2(distance * jumpMultiplier, jumpPower + amount);
        onAttack = true;
        animator.Play("Jump");
    }

    public void Landing()
    {
        rigid.velocity = Vector2.zero;
        onAttack = false;
        onHit = false;
    }

    public void ToAttackState()
    {
        if (attackRoutine != null)
            return;

        stateMachine.ChangeState(State.Attack);
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject, dieTime);
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(1f);
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onAttack && (((1 << collision.gameObject.layer) & PlayerLayer) != 0))
        {
            player.TakeDamage(damage);
            player.Knockback(transform.position, hitPower);
        }
    }
}
