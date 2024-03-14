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
}
