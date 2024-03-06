using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator animator;
    [SerializeField] PlayerInput input;

    [Header("status")]
    [SerializeField] int hp;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;

    [Header("Physics")]
    [SerializeField] float accel;
    [SerializeField] float multiplier;
    [SerializeField] float lowJumpMultiplier;
    [SerializeField] float maxFall;

    Vector2 moveDir;
    private float moveSpeed;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        // "Run"키를 누르고 움직이면 달리기
        moveSpeed = input.actions["Run"].IsPressed() ? runSpeed : walkSpeed;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // 목표속도 구하기
        float target = moveDir.x * moveSpeed;
        // 현재속도와 목표속도 차이
        float diffSpeed = target - rigid.velocity.x;
        // 현재속도와 목표속도의 차이를 가속 가중치로 설정하여 최대속도 설정
        rigid.AddForce(Vector3.right * diffSpeed * accel);

        animator.SetFloat("MoveSpeed", Mathf.Abs(rigid.velocity.x));

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

        // 플레이어 방향바꾸기
        if (moveDir.x != 0)
        {
            transform.localScale = new Vector3(moveDir.x, 1, 1);
        }
    }

    private void Jump()
    {
        rigid.velocity = new Vector3(rigid.velocity.x, jumpSpeed);
    }

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        Jump();
    }
}
