using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IDamagable
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprites;
    [SerializeField] int maxHp;
    [SerializeField] int hp;
    [SerializeField] int mass;

    public int Mass => mass;

    private void Start()
    {
        hp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        hp--;
        int index = sprites.Length - (int)((float)sprites.Length * hp / maxHp) - 1;
        spriteRenderer.sprite = sprites[index];

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Knockback(Vector2 hitPoint, float hitPower)
    {
        float direction = Mathf.Sign(transform.position.x - hitPoint.x);
        Vector2 knockback = new Vector2(direction, 0.5f).normalized;
        rigid.velocity = knockback * hitPower;
    }
}
