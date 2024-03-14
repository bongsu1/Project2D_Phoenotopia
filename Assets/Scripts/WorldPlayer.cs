using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldPlayer : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] float moveSpeed;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask entranceLayer; // ÀÔ±¸

    Vector2 moveDir;

    private void Update()
    {
        if (rigid.velocity.magnitude == 0)
        {
            animator.speed = 0f;
            return;
        }

        animator.speed = 1f;
        if (Mathf.Abs(rigid.velocity.x) > 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(rigid.velocity.x), 1f, 1f);
            animator.Play("WorldRight");
        }
        else if (rigid.velocity.y > 0)
        {
            animator.Play("WorldUp");
        }
        else if ( rigid.velocity.y < 0)
        {
            animator.Play("WorldDown");
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigid.velocity = moveDir.normalized * moveSpeed;
    }

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }
}
