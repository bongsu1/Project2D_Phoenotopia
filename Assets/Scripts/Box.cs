using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    [SerializeField] Rigidbody2D rigid;

    public void TakeDamage(int damage)
    {
        //hp -= damage;

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
