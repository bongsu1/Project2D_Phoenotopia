using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldEnemy : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator animator;
    [SerializeField] float moveSpeed;
    [SerializeField] float activeTime;
    [SerializeField] LayerMask enemyLayer;

    private enum Patrol { Height, Width }
    private Patrol curPatrol;
    private float direction;
    private bool isIdle;
    private float activeTimer;

    private void OnEnable()
    {
        activeTimer = activeTime;
        int rand = Random.Range(0, 2);
        curPatrol = (Patrol)rand;
        StartCoroutine(PatrolRoutine());
    }

    private void Update()
    {
        activeTimer -= Time.deltaTime;
        if (activeTimer <= 0)
        {
            gameObject.SetActive(false);
        }

        if (rigid.velocity.magnitude < 0.1f)
        {
            animator.speed = 0f;
            return;
        }

        animator.speed = 1f;
        transform.localScale = new Vector2(Mathf.Sign(rigid.velocity.x), 1f);

        if (Mathf.Abs(rigid.velocity.x) > 0f)
        {
            animator.Play("WorldRight");
        }
        else if (rigid.velocity.y > 0f)
        {
            animator.Play("WorldUp");
        }
        else if (rigid.velocity.y < 0f)
        {
            animator.Play("WorldDown");
        }
    }

    private void FixedUpdate()
    {
        if (isIdle)
        {
            rigid.velocity = Vector2.zero;
            return;
        }

        Move();
    }

    private void Move()
    {
        switch (curPatrol)
        {
            case Patrol.Height:
                rigid.velocity = Vector2.up * direction * moveSpeed;
                break;
            case Patrol.Width:
                rigid.velocity = Vector2.right * direction * moveSpeed;
                break;
        }
    }

    IEnumerator PatrolRoutine()
    {
        direction = Random.Range(0, 2) == 0 ? 1 : -1;

        while (true)
        {
            float time = Random.Range(1f, 3f);
            while (time > 0f)
            {
                time -= Time.deltaTime;
                yield return null;
            }

            isIdle = true;
            yield return new WaitForSeconds(0.5f);
            isIdle = false;
            direction *= -1f;
            curPatrol = (Patrol)Random.Range(0, 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
            return;

        isIdle = true;
    }
}
