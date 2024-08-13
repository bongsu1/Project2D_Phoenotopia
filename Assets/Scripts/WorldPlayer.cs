using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WorldPlayer : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator animator;
    [SerializeField] PlayerInput input;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] LayerMask entranceLayer; // �Ա�
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float useStamina;

    public UnityEvent OnAttackKeyPress;
    public UnityEvent<Vector2> OnEnemyTouch;

    private Vector2 moveDir;
    private float moveSpeed;
    private bool onEntrance;
    private bool onEnemyTouch;

    public PlayerInput Input => input;

    private void Update()
    {
        moveSpeed = input.actions["Run"].IsPressed() && Manager.Data.Stamina > 0f ? runSpeed : walkSpeed;

        if (input.actions["Run"].IsPressed() && input.actions["Run"].triggered)
        {
            Manager.Data.Stamina -= useStamina;
            Manager.Data.StopStaminaRegenRoutine();
        }
        else if (input.actions["Run"].IsPressed() && moveDir.magnitude > 0)
        {
            Manager.Data.Stamina -= Time.deltaTime;
        }
        else if (!input.actions["Run"].IsPressed() && input.actions["Run"].triggered)
        {
            Manager.Data.StartStaminaRegenRoutine();
        }

        if (onEnemyTouch || (moveDir.magnitude < 0.1f))
        {
            rigid.velocity = Vector2.zero;
            animator.speed = 0f;
            return;
        }

        animator.speed = 1f;
        if (Mathf.Abs(moveDir.x) > 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveDir.x), 1f, 1f);
            animator.Play("WorldRight");
        }
        else if (moveDir.y > 0)
        {
            animator.Play("WorldUp");
        }
        else if (moveDir.y < 0)
        {
            animator.Play("WorldDown");
        }
    }

    private void FixedUpdate()
    {
        if (onEnemyTouch)
            return;

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

    // ���� �ѹ��� �ε��ϱ� ���ػ��
    int loadCount = 1;
    private void OnAttack(InputValue value)
    {
        if (onEntrance && value.isPressed && loadCount > 0)
        {
            loadCount--;
            OnAttackKeyPress?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & entranceLayer) != 0)
        {
            onEntrance = true;
        }
        else if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            onEnemyTouch = true;
            OnEnemyTouch?.Invoke(collision.ClosestPoint(transform.position));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & entranceLayer) != 0)
        {
            onEntrance = false;
        }
    }
}
