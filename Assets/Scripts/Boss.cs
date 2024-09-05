using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Boss : MonoBehaviour, IDamagable
{
    public enum State { Idle, Attack, Die }
    private StateMachine<State> stateMachine = new StateMachine<State>();

    [Header("Stat")]
    [SerializeField] int hp;

    [SerializeField] int attackDamage;
    public int AttackDamage { get { return attackDamage; } }

    [SerializeField] float attackRange;
    public float AttackRange { get { return attackRange; } }

    [Header("Component")]
    [SerializeField] Rigidbody2D rigid;
    public Rigidbody2D Rigid { get { return rigid; } }

    [SerializeField] Collider2D coll;
    public Collider2D Coll { get { return coll; } }

    [SerializeField] Animator anim;

    [Header("player")]
    [SerializeField] Player player;
    public Player Player { get { return player; } }

    [SerializeField] LayerMask playerLayer;
    public LayerMask PlayerLayer { get { return playerLayer; } }

    private void Start()
    {
        Physics2D.IgnoreCollision(coll, player.PlayerColl);

        stateMachine.AddState(State.Idle, new B_IdleState(this));
        stateMachine.AddState(State.Attack, new B_AttackState(this));
        stateMachine.AddState(State.Die, new B_DieState(this));

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

    public void TakeDamage(int damage)
    {
        if (hp == 0)
            return;

        hp -= damage;

        if (hp <= 0)
        {
            stateMachine.ChangeState(State.Die);
        }
    }

    public void Knockback(Vector2 hitPoint, float hitPower)
    {
        // 넉백되지 않음
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(attackDamage);
                damagable.Knockback(transform.position, 1f);
            }

            Physics2D.IgnoreCollision(coll, collision.collider);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

#region State
public class BossState : BaseState<Boss.State>
{
    protected Boss boss;

    public BossState(Boss boss)
    {
        this.boss = boss;
    }
}

public class B_IdleState : BossState
{
    WaitForSeconds wait = new WaitForSeconds(3f);
    Coroutine waitRoutine;

    public override void Enter()
    {
        waitRoutine = boss.StartCoroutine(WaitRoutine());
    }

    public override void Exit()
    {
        if (waitRoutine != null)
        {
            boss.StopCoroutine(waitRoutine);
        }
    }

    IEnumerator WaitRoutine()
    {
        yield return wait;
        waitRoutine = null;
        ChangeState(Boss.State.Attack);
    }

    public B_IdleState(Boss boss) : base(boss) { }
}

public class B_AttackState : BossState
{
    WaitForSeconds waitAttack = new WaitForSeconds(1f);
    Coroutine attackRoutine;

    public override void Enter()
    {
        if (Vector3.Distance(boss.transform.position, boss.Player.transform.position) < boss.AttackRange)
        {
            attackRoutine = boss.StartCoroutine(AllRoundAttack());
        }
        else
        {
            attackRoutine = boss.StartCoroutine(JumpAttack());
        }
    }

    public override void Exit()
    {
        if (attackRoutine != null)
        {
            boss.StopCoroutine(attackRoutine);
        }
    }

    IEnumerator JumpAttack()
    {
        Jump(boss.Player.transform.position);
        yield return waitAttack;
        boss.Rigid.velocity = Vector2.zero;
        attackRoutine = null;
        ChangeState(Boss.State.Idle);
    }

    IEnumerator AllRoundAttack()
    {
        RoundAttack();
        yield return waitAttack;
        attackRoutine = null;
        ChangeState(Boss.State.Idle);
    }

    private void Jump(Vector2 playerPos)
    {
        float distance = playerPos.x - boss.transform.position.x;
        boss.Rigid.velocity = new Vector2(distance, 5f);
        Physics2D.IgnoreCollision(boss.Coll, boss.Player.PlayerColl, false);
    }

    Collider2D[] collider = new Collider2D[10];
    private void RoundAttack()
    {
        int size = Physics2D.OverlapCircleNonAlloc(boss.transform.position, boss.AttackRange,
            collider, boss.PlayerLayer);
        if (size >= 1)
        {
            IDamagable damagable = collider[1].GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(boss.AttackDamage);
                damagable.Knockback(boss.transform.position, 1f);
            }
        }
    }

    public B_AttackState(Boss boss) : base(boss) { }
}

public class B_DieState : BossState
{
    public B_DieState(Boss boss) : base(boss) { }
}
#endregion