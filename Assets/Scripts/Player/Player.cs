using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class Player : MonoBehaviour, IDamagable
{
    public enum State { Sleep, Normal, Jump, Duck, Climb, Attack, Charge, Grab, Carry, Talk, Use, Hit, Die }
    private StateMachine<State> stateMachine = new StateMachine<State>();

    [Header("Component")]
    [SerializeField] Rigidbody2D rigid;
    public Rigidbody2D Rigid { get { return rigid; } }

    [SerializeField] Animator animator;
    public Animator Animator { get { return animator; } }

    [SerializeField] PlayerInput input;
    public PlayerInput Input { get { return input; } }

    [SerializeField] BoxCollider2D playercoll; // ���̴� �ڼ����� �ݶ��̴� ����� ���̱� ���� BoxCollider2D�� �ʿ�
    public BoxCollider2D PlayerColl { get { return playercoll; } }

    [SerializeField] Transform attackPoint; // ���ݹ��� ��ġ
    [SerializeField] Transform grabPoint; // ���ڸ� ��� ��ġ
    [SerializeField] Transform interactPoint; // NPC ��ȣ�ۿ� ����Ʈ

    [SerializeField] Transform slingshotAim;
    public Transform SlingshotAim { get { return slingshotAim; } }

    [SerializeField] PlayerHeadCheck headCheck;

    [SerializeField] PixelPerfectCamera pixel;
    public PixelPerfectCamera Pixel { get { return pixel; } }

    [SerializeField] PlayerEffect effect;

    [SerializeField] PlayerSoundManager sfx;
    public PlayerSoundManager SFX { get { return sfx; } }

    [Header("status")]
    [SerializeField] int damage;

    [SerializeField] float useStamina;
    public float UseStamina { get { return useStamina; } }

    [Header("Normal")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    [SerializeField] float jumpSpeed;
    public float JumpSpeed { get { return jumpSpeed; } }

    [Header("State move speed")]
    [SerializeField] float duckMoveSpeed;
    public float DuckMoveSpeed { get { return duckMoveSpeed; } }

    [SerializeField] float climbMoveSpeed;
    public float ClimbMoveSpeed { get { return climbMoveSpeed; } }

    [SerializeField] float chargedMoveSpeed;
    public float ChargedMoveSpeed { get { return chargedMoveSpeed; } }

    [SerializeField] float grabMoveSpeed;
    public float GrabMoveSpeed { get { return grabMoveSpeed; } }

    [Header("Attack")]
    [SerializeField] float normalAttackRange;
    [SerializeField] float chargeAttackRange;
    [SerializeField] float normalHitPower;
    [SerializeField] float chargeHitPower;

    [Header("Carry")]
    [SerializeField] float grabRange;

    [SerializeField] float throwPower;
    public float ThrowPower { get { return throwPower; } }

    [Header("Use")]
    [SerializeField] float aimSpeed;
    public float AimSpeed { get { return aimSpeed; } }

    [SerializeField] float slingshotPower;

    [Header("Physics")]
    [SerializeField] float accel;
    public float Accel { get { return accel; } }

    [SerializeField] float multiplier;
    [SerializeField] float lowJumpMultiplier;
    [SerializeField] float maxFall;

    [SerializeField] float takeHitPower;
    public float TakeHitPower { get { return takeHitPower; } set { takeHitPower = value; } }

    [SerializeField] PhysicsMaterial2D playerMaterial;
    public PhysicsMaterial2D PlayerMaterial { get { return playerMaterial; } }

    [SerializeField] float bounciness;
    public float Bounciness { get { return bounciness; } }

    [Header("LayerMask")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] LayerMask LadderLayer;
    [SerializeField] LayerMask damagableLayer;
    [SerializeField] LayerMask grabbableLayer;
    [SerializeField] LayerMask npcLayer;
    [SerializeField] Vector2 interactBoxSize;
    [SerializeField] LayerMask doorLayer;

    [Header("Prefab")]
    [SerializeField] Bullet slingshotBullet;

    Vector2 moveDir;
    public Vector2 MoveDir { get { return moveDir; } }

    private float normalSpeed;
    public float NormalSpeed { get { return normalSpeed; } }

    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    private float chargeTime;
    public float ChargeTime { get { return chargeTime; } set { chargeTime = value; } }

    private float attackRange;

    private float hitPower;
    public float HitPower { get { return hitPower; } }

    private float doorXPosition;
    private int groundCount;
    private int ladderCount;

    private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }

    private bool isDucking;
    public bool IsDucking { get { return isDucking; } }

    private bool isLadder;
    public bool IsLadder { get { return isLadder; } }

    private bool onNPC;
    public bool OnNPC { get { return onNPC; } }

    private bool onTalk;
    public bool OnTalk { get { return onTalk; } set { onTalk = value; } }

    private bool onDoor;
    public bool OnDoor { get { return onDoor; } }

    private bool onEnter;
    public bool OnEnter { get { return onEnter; } }

    private bool onExit;
    private bool onHit;
    public bool OnHit { get { return onHit; } set { onHit = value; } }

    private bool onCeiling; // �Ӹ����� õ��üũ
    public bool OnCeiling { get { return onCeiling; } }

    private Collider2D platformcoll;

    private Box box;
    public Box Box { get { return box; } set { box = value; } }

    [Header("Event")]
    public UnityEvent OnWakeUp;
    public UnityEvent OnNoramalAttack;

    private void Start()
    {
        stateMachine.AddState(State.Sleep, new SleepState(this));
        stateMachine.AddState(State.Normal, new NormalState(this)); // merge(Idle, walk)
        stateMachine.AddState(State.Jump, new JumpState(this));
        stateMachine.AddState(State.Duck, new DuckState(this));
        stateMachine.AddState(State.Climb, new ClimbState(this));
        stateMachine.AddState(State.Attack, new AttackState(this));
        stateMachine.AddState(State.Charge, new ChargeState(this));
        stateMachine.AddState(State.Grab, new GrabState(this));
        stateMachine.AddState(State.Carry, new CarryState(this));
        stateMachine.AddState(State.Talk, new TalkState(this));
        stateMachine.AddState(State.Use, new UseState(this));
        stateMachine.AddState(State.Hit, new HitState(this));
        stateMachine.AddState(State.Die, new DieState(this));

        stateMachine.Start(State.Normal);
    }

    float xPosition;
    private void Update()
    {
        if (onEnter)
        {
            if (onExit)
                return;

            xPosition = Mathf.Lerp(transform.position.x, doorXPosition, 0.1f);
            transform.position = new Vector2(xPosition, transform.position.y);
            return;
        }

        isGrounded = groundCount > 0;
        isDucking = moveDir.y < -0.1f && isGrounded;
        isLadder = ladderCount > 0;
        onCeiling = headCheck.OnCeiling;

        if (input.actions["Run"].IsPressed())
        {
            normalSpeed = runSpeed;
        }
        else
        {
            normalSpeed = walkSpeed;
        }

        // ��¡�� �Ǹ� hitPower�� ������ attackRange�� Ŀ����
        hitPower = chargeTime > 1f ? chargeHitPower : normalHitPower;
        attackRange = chargeTime > 1f ? chargeAttackRange : normalAttackRange;

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (onEnter)
            return;

        stateMachine.FixedUpdate();

        // ª�� ���� ����
        if (rigid.velocity.y < 0)
        {
            //                                                   multiplier = 2.5f
            rigid.velocity += Vector2.up * Physics2D.gravity.y * multiplier * Time.deltaTime;
        }
        else if (rigid.velocity.y > 0 && !input.actions["Jump"].IsPressed())
        {
            //                                                   lowJumpMultiplier = 2f
            rigid.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }

        // �������� �ִ�ӵ� ����
        if (rigid.velocity.y < -maxFall)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -maxFall);
        }
    }

    Collider2D[] colliders = new Collider2D[10];
    public void Attack()
    {
        int size = Physics2D.OverlapCircleNonAlloc(attackPoint.position, attackRange, colliders, damagableLayer);
        for (int i = 0; i < size; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
                damagable.Knockback(transform.position, hitPower);
            }
        }

        if (size > 0)
        {
            effect.StartHitEffectRoutine(colliders[0].ClosestPoint(attackPoint.position) + new Vector2(transform.localScale.x * .25f, 0));
            sfx.PlaySFX(PlayerSoundManager.SFX.Hit);
        }
    }

    public void Grab()
    {
        int size = Physics2D.OverlapCircleNonAlloc(grabPoint.position, grabRange, colliders, grabbableLayer);
        if (size > 0)
        {
            colliders[0].transform.SetParent(transform);
            box = colliders[0].GetComponent<Box>();
        }
    }

    public void Talk()
    {
        int size = Physics2D.OverlapBoxNonAlloc(interactPoint.position, interactBoxSize, 0, colliders, npcLayer);
        if (size > 0)
        {
            onTalk = true;
            IInteractable npc = colliders[0].GetComponent<IInteractable>();
            if (npc != null)
            {
                npc.Interact(this);
            }
            else if (npc == null)
            {
                onTalk = false;
            }
        }
    }

    IEnterable door;
    public void EnterDoor()
    {
        int size = Physics2D.OverlapBoxNonAlloc(interactPoint.position, interactBoxSize, 0, colliders, doorLayer);
        if (size > 0)
        {
            door = colliders[0].GetComponent<IEnterable>();
            if (door != null)
            {
                doorXPosition = colliders[0].transform.position.x;
                door.Enter(this);
            }
        }
    }

    public void ExitDoor()
    {
        door.Exit(this);
        onExit = true;
    }

    public void IsEnter()
    {
        Manager.UI.CloseTutorialUI();
        onEnter = true;
    }

    public void IsExit()
    {
        onDoor = true;
        onEnter = false;
        door = null;
        onExit = false;
    }

    // ������ �����Ҷ� ������ ȣ��
    public void StartGame()
    {
        stateMachine.ChangeState(State.Sleep);
    }

    // DieScene���� ȣ���
    public void EndGame()
    {
        stateMachine.ChangeState(State.Die);
    }

    // ���� �ִϸ��̼��� ������ �������� �ִϸ��̼� �̺�Ʈ�� ȣ��
    // + ��� ������ ���оִϸ��̼� ������ ȣ��
    public void ToNormalState()
    {
        stateMachine.ChangeState(State.Normal);
    }

    // ������ ��� �ø��� �ִϸ��̼��� ������ ȣ��
    public void ToCarryState()
    {
        if (box.Mass > 4)
            return;

        stateMachine.ChangeState(State.Carry);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(grabPoint.position, grabRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(interactPoint.position, interactBoxSize);
    }

    // ���ѿܿ� ��°��� �� ����� �װ� �ҷ��ͼ� ��°��� ����
    public void Shot()
    {
        Vector2 shotDir = transform.localScale.x > 0 ? slingshotAim.right : -slingshotAim.right;
        Rigidbody2D bullet = Instantiate(slingshotBullet, slingshotAim.position, slingshotAim.rotation).GetComponent<Rigidbody2D>();
        bullet.velocity = shotDir * slingshotPower;
    }

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (isDucking && platformcoll)
        {
            StartCoroutine(DownJumpRoutine());
        }
    }

    IEnumerator DownJumpRoutine()
    {
        Physics2D.IgnoreCollision(playercoll, platformcoll);
        yield return new WaitForSeconds(0.15f);
        Physics2D.IgnoreCollision(playercoll, platformcoll, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            groundCount++;
            if (((1 << collision.gameObject.layer) & platformLayer) != 0)
            {
                platformcoll = collision.gameObject.GetComponent<Collider2D>();
            }
        }
        else if (((1 << collision.gameObject.layer) & LadderLayer) != 0)
        {
            //Manager.UI.ShowTutorialUI(transform, TutorialType.Climb);
            ladderCount++;
        }
        else if (((1 << collision.gameObject.layer) & npcLayer) != 0)
        {
            Manager.UI.ShowTutorialUI(transform, TutorialType.Talk);
            onNPC = true;
        }
        else if (((1 << collision.gameObject.layer) & doorLayer) != 0)
        {
            Manager.UI.ShowTutorialUI(transform, TutorialType.Enter);
            onDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            groundCount--;
        }
        else if (((1 << collision.gameObject.layer) & LadderLayer) != 0)
        {
            //Manager.UI.CloseTutorialUI();
            ladderCount--;
        }
        else if (((1 << collision.gameObject.layer) & npcLayer) != 0)
        {
            Manager.UI.CloseTutorialUI();
            onNPC = false;
        }
        else if (((1 << collision.gameObject.layer) & doorLayer) != 0)
        {
            Manager.UI.CloseTutorialUI();
            onDoor = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (onHit)
            return;

        if (Manager.Data.Hp - damage < 0)
        {
            Manager.Data.Hp = 0;
        }
        else
        {
            Manager.Data.Hp -= damage;
        }
    }

    public void Knockback(Vector2 hitPoint, float hitPower)
    {
        if (onHit)
            return;

        if (Manager.Data.Hp < 0)
            return;

        onHit = true;
        float direction = Mathf.Sign(transform.position.x - hitPoint.x);
        Vector2 knockback = new Vector2(direction, hitPower > 9 ? hitPower * 0.5f : 0.5f).normalized;
        rigid.velocity = knockback * hitPower;
        transform.localScale = new Vector3(-direction, 1f, 1f);

        takeHitPower = hitPower;

        stateMachine.ChangeState(State.Hit);
        Die();
    }

    Coroutine knockbackRoutine;
    public void StartKnockbackRoutine(float takeHitPower)
    {
        knockbackRoutine = StartCoroutine(KnockbackRoutine(takeHitPower));
    }

    IEnumerator KnockbackRoutine(float takeHitPower)
    {
        yield return new WaitForSeconds(takeHitPower * 0.25f);
        ToNormalState();
    }

    public void StopKnockbackRoutine()
    {
        StopCoroutine(knockbackRoutine);
        onHit = false;
    }

    public void Die()
    {
        if (Manager.Data.Hp > 0)
            return;

        Manager.Scene.LoadScene("DieScene");
    }

    private void OnDisable()
    {
        playerMaterial.bounciness = 0f;
    }
}
