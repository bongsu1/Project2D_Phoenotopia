using UnityEngine;

public class Box : MonoBehaviour, IDamagable
{
    [SerializeField] Rigidbody2D rigid;
    public Rigidbody2D Rigid { get { return rigid; } }

    [SerializeField] BoxCollider2D boxColl;
    public BoxCollider2D BoxColl { get { return boxColl; } }

    [SerializeField] SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRender { get { return spriteRenderer; } }

    [SerializeField] Sprite[] sprites;
    [SerializeField] int maxHp;
    [SerializeField] int hp;

    [SerializeField] int mass;
    public int Mass { get { return mass; } }

    [SerializeField] bool canNockback;

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
        if (!canNockback)
            return;

        float direction = Mathf.Sign(transform.position.x - hitPoint.x);
        Vector2 knockback = new Vector2(direction, 0.5f).normalized;
        rigid.velocity = knockback * hitPower;
    }
}
