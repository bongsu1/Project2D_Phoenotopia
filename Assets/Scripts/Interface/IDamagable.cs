using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(int damage);

    public void Knockback(Vector2 hitPoint, float hitPower);
}
