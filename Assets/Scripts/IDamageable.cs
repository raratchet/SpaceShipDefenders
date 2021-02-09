using UnityEngine;

public interface IDamageable 
{
    bool IsAlive();
    void Damage(GameObject damager ,float damage);
}
