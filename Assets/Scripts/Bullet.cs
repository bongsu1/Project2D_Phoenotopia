using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] float time;
    [SerializeField] float hitPower;

    private void Start()
    {
        Destroy(gameObject, time);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(damage);
            damagable.Knockback(transform.position, hitPower);
            Destroy(gameObject);
        }
    }
}
