using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public NavMeshAgent nav;
    public Animator anim;
    public float baseHealth;
    public float baseSpeed;
    public float baseRange;
    public float range;
    public float damage;
    public GameObject objective;
    public float attackCooldown;
    public float lastAttack;

    public float health;
    public float speed;
    public bool isDead;

    public abstract void Attack();
    public void Damage(GameObject damager,float damage)
    {
        health -= damage;
        if (health <= 0  && !isDead)
        {
            isDead = true;
            StartCoroutine(Die());
        }
    }

    public bool IsAlive()
    {
        return !isDead;
    }
    public abstract IEnumerator Die();
    public abstract void Mock();
    public abstract void Movement(Vector3 destination);
}
