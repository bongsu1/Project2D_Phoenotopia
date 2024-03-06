using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum State { Sleep, Normal, Jump, Duck, Climb }

    private StateMachine<State> stateMachine = new StateMachine<State>();

    [Header("Component")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator animator;
    [SerializeField] PlayerInput input;

    [Header("status")]
    [SerializeField] int hp;
    [SerializeField] int stamina;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float duckMoveSpeed;
    [SerializeField] float climbMoveSpeed;

    [Header("Physics")]
    [SerializeField] float accel;
    [SerializeField] float multiplier;
    [SerializeField] float lowJumpMultiplier;
    [SerializeField] float maxFall;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] LayerMask LadderLayer;

    Vector2 moveDir;
    private float moveSpeed;
    private int groundCount;
    private int ladderCount;
    private bool isGrounded;
    private bool isDucking;
    private bool isLadder;
    Collider2D platformcoll;
    BoxCollider2D playercoll;

    // Property
    public Animator Animator => animator;
    public PlayerInput Input => input;
    public Vector2 MoveDir => moveDir;
    public Rigidbody2D Rigid => rigid;
    public BoxCollider2D PlayerColl => playercoll;
    public Collider2D PlatformColl => platformcoll;

    public float Accel => accel;
    public float JumpSpeed => jumpSpeed;
    public float MoveSpeed => moveSpeed;
    public float ClimbMoveSpeed => climbMoveSpeed;

    public bool IsGrounded => isGrounded;
    public bool IsDucking => isDucking;
    public bool IsLadder => isLadder;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        playercoll = GetComponent<BoxCollider2D>();

        stateMachine.AddState(State.Sleep, new SleepState(this));
        stateMachine.AddState(State.Normal, new NormalState(this)); // merge(Idle, walk)
        stateMachine.AddState(State.Jump, new JumpState(this));
        stateMachine.AddState(State.Duck, new DuckState(this));
        stateMachine.AddState(State.Climb, new ClimbState(this));

        stateMachine.Start(State.Normal); // Sleep으로 시작
    }

    private void Update()
    {
        isGrounded = groundCount > 0;
        isDucking = moveDir.y < -0.1f && isGrounded;
        isLadder = ladderCount > 0;

        // 숙인 상태면 느리게
        if (isDucking)
        {
            moveSpeed = duckMoveSpeed;
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

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
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

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (isDucking)
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
    }
}
