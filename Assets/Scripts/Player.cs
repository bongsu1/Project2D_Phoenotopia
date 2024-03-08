using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum State { Sleep, Normal, Jump, Duck, Climb, Attack, Charge, Grab, Carry, Talk, Use }

    private StateMachine<State> stateMachine = new StateMachine<State>();

    [Header("Component")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator animator;
    [SerializeField] PlayerInput input;
    [SerializeField] BoxCollider2D playercoll; // 숙이는 자세에서 콜라이더 사이즈를 줄이기 위해 BoxCollider2D가 필요
    [SerializeField] Transform attackPoint; // 공격범위 위치
    [SerializeField] Transform grabPoint; // 상자를 잡는 위치
    [SerializeField] Transform interactPoint; // NPC 상호작용 포인트
    [SerializeField] Transform slingShotAim;

    [Header("status")]
    [SerializeField] int damage;
    [SerializeField] int hp;
    [SerializeField] int stamina;

    [Header("Normal")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;

    [Header("State move speed")]
    [SerializeField] float duckMoveSpeed;
    [SerializeField] float climbMoveSpeed;
    [SerializeField] float ChargerMoveSpeed;
    [SerializeField] float grabMoveSpeed;

    [Header("Attack")]
    [SerializeField] float normalAttackRange;
    [SerializeField] float chargeAttackRange;
    [SerializeField] float normalHitPower;
    [SerializeField] float chargeHitPower;

    [Header("Carry")]
    [SerializeField] float grabRange;
    [SerializeField] float throwPower;

    [Header("Physics")]
    [SerializeField] float accel;
    [SerializeField] float multiplier;
    [SerializeField] float lowJumpMultiplier;
    [SerializeField] float maxFall;

    [Header("LayerMask")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] LayerMask LadderLayer;
    [SerializeField] LayerMask damagableLayer;
    [SerializeField] LayerMask grabbableLayer;
    [SerializeField] LayerMask npcLayer;
    [SerializeField] Vector2 interactBoxSize;
    [SerializeField] LayerMask doorLayer;

    //test
    public float useTime;

    Vector2 moveDir;
    private float moveSpeed;
    private float chargeTime;
    private float attackRange;
    private float hitPower;
    private float doorXPosition;
    private float aimRotateAngle;
    private int groundCount;
    private int ladderCount;
    private bool isGrounded;
    private bool isDucking;
    private bool isLadder;
    private bool onNPC;
    private bool onTalk;
    private bool onDoor;
    private bool onEnter;
    Collider2D platformcoll;
    Box box;

    // Property
    public Animator Animator => animator;
    public PlayerInput Input => input;
    public Vector2 MoveDir => moveDir;
    public Rigidbody2D Rigid => rigid;
    public BoxCollider2D PlayerColl => playercoll;
    public Box Box { get { return box; } set { box = value; } }

    public float Accel => accel;
    public float JumpSpeed => jumpSpeed;
    public float MoveSpeed => moveSpeed;
    public float ClimbMoveSpeed => climbMoveSpeed;
    public float ChargeTime { get { return chargeTime; } set { chargeTime = value; } }
    public float ThrowPower => throwPower;
    public float HitPower => hitPower;
    public float AimRotateAngle { get { return aimRotateAngle; } set { aimRotateAngle = value; } }

    public bool IsGrounded => isGrounded;
    public bool IsDucking => isDucking;
    public bool IsLadder => isLadder;
    public bool OnNPC => onNPC;
    public bool OnTalk { get { return onTalk; } set { onTalk = value; } }
    public bool OnDoor => onDoor;
    public bool OnEnter => onEnter;

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

        //stateMachine.Start(State.Sleep); // Sleep으로 시작
        stateMachine.Start(State.Normal);
    }

    private void Update()
    {
        if (onEnter)
        {
            xPosition = Mathf.Lerp(transform.position.x, doorXPosition, 0.1f);
            transform.position = new Vector2(xPosition, transform.position.y);
            return;
        }

        isGrounded = groundCount > 0;
        isDucking = moveDir.y < -0.1f && isGrounded;
        isLadder = ladderCount > 0;

        // 숙인 상태면 느려지게
        if (isDucking)
        {
            moveSpeed = duckMoveSpeed;
        }
        // 공격 차지중에는 느려지게
        else if (input.actions["Attack"].IsPressed())
        {
            moveSpeed = ChargerMoveSpeed;
        }
        // 물건을 밀거나 끌때는 느려지게
        else if (input.actions["Grab"].IsPressed() && box != null)
        {
            moveSpeed = grabMoveSpeed;
        }
        // "Run"키를 누르고 움직이면 달리기
        else if (input.actions["Run"].IsPressed())
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        // 차징이 되면 hitPower가 쎄지고 attackRange가 커진다
        hitPower = chargeTime > 1f ? chargeHitPower : normalHitPower;
        attackRange = chargeTime > 1f ? chargeAttackRange : normalAttackRange;

        stateMachine.Update();
    }

    float xPosition;
    private void FixedUpdate()
    {
        if (onEnter)
            return;

        stateMachine.FixedUpdate();

        // 숏컷 점프 구현
        if (rigid.velocity.y < 0)
        {
            rigid.velocity += Vector2.up * Physics2D.gravity.y * multiplier * Time.deltaTime;
        }
        else if (rigid.velocity.y > 0 && !input.actions["Jump"].IsPressed())
        {
            rigid.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }

        // 떨어지는 최대속도 설정
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
                Rigidbody2D other = colliders[i].GetComponent<Rigidbody2D>();
                if (other != null)
                {
                    Vector2 hitDir = new Vector2(other.position.x - transform.position.x, other.position.y - transform.position.y).normalized;
                    other.velocity = hitDir * hitPower;
                }
                damagable.TakeDamage(damage);
            }
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
    }

    public void IsEnter()
    {
        onEnter = true;
    }

    public void IsExit()
    {
        onDoor = true;
        onEnter = false;
        door = null;
    }

    // 공격 애니메이션이 끝나는 지점에서 애니메이션 이벤트로 호출
    // + 잡기 실패후 실패애니메이션 실행후 호출
    public void ToNormalState()
    {
        stateMachine.ChangeState(State.Normal);
    }

    // 물건을 잡아 올리는 애니메이션이 끝나고 호출
    public void ToCarryState()
    {
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

    //
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
        yield return new WaitForSeconds(0.2f);
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
            ladderCount++;
        }
        else if (((1 << collision.gameObject.layer) & npcLayer) != 0)
        {
            onNPC = true;
        }
        else if (((1 << collision.gameObject.layer) & doorLayer) != 0)
        {
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
            ladderCount--;
        }
        else if (((1 << collision.gameObject.layer) & npcLayer) != 0)
        {
            onNPC = false;
        }
        else if (((1 << collision.gameObject.layer) & doorLayer) != 0)
        {
            onDoor = false;
        }
    }
}
