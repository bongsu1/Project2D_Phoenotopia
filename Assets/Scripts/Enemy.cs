using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("Component")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected CircleCollider2D playerCheck;

    [Header("Status")]
    [SerializeField] protected int hp;
    [SerializeField] protected int damage;

    [Header("Physics")]
    [SerializeField] protected float checkSize;
    [SerializeField] LayerMask playerLayer;

    protected Player player;
    protected bool onPlayerCheck;
    private int attackCount = 1;


    // property
    public Animator Animator => animator;
    public Rigidbody2D Rigid => rigid;
    public Player Player => player;
    public bool OnPlayerCheck { get { return onPlayerCheck; } set { onPlayerCheck = value; } }
    public int AttackCount { get { return attackCount; } set { attackCount = value; } }
    public float CheckSize => checkSize;
    public LayerMask PlayerLayer => playerLayer;

    public virtual void TakeDamage(int damage)
    {
        hp -= damage;
    }

    public virtual void Knockback(Vector2 hitPoint, float hitPower)
    {
        if (hp < 0)
            return;

        float direction = Mathf.Sign(transform.position.x - hitPoint.x);
        Vector2 knockback = new Vector2(direction, 0.5f).normalized;
        rigid.velocity = knockback * hitPower;

        if (player == null)
        {
            player = FindObjectOfType<Player>();
            playerCheck.gameObject.SetActive(false);
        }
    }

    public void PlayerCheck()
    {
        if (onPlayerCheck)
            return;

        if (player == null)
            return;

        // 플레이어의 기준점이 발로 되어 있어서 0.4만큼 더함
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y + 0.4f);
        if (Vector2.Distance(playerPos, transform.position) > checkSize)
            return;

        if (((player.transform.position.x - transform.position.x) > 0) // 플레이어가 오른쪽에 있을때
            && transform.localScale.x < 0) // 왼쪽을 바라볼 때
            return;
        else if (((player.transform.position.x - transform.position.x) < 0) // 플레이어가 왼쪽에 있을 때
            && transform.localScale.x > 0) // 오른쪽을 바라볼 때
            return;

        onPlayerCheck = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player != null)
            return;

        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            player = collision.GetComponent<Player>();
            playerCheck.gameObject.SetActive(false);
        }
    }
}
