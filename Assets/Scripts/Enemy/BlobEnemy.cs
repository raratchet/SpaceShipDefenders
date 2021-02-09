using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BlobEnemy : Enemy
{
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.stoppingDistance = range;
    }

    void OnEnable()
    {
        objective = GameObject.FindGameObjectWithTag("Ship");
        Movement(objective.transform.position);
        health = baseHealth;
        speed = baseSpeed;
        isDead = false;
    }

    void Update()
    {
        if(GameManager.Instance.gameOver)
        {
            nav.isStopped = true;
            return;
        }
        if (range >= Vector3.Distance(transform.position, objective.transform.position))
        {
            if(lastAttack + attackCooldown < Time.time)
            {
                Attack();
                lastAttack = Time.time;
            }
        }
    }

    public override void Attack()
    {
        if(!isDead)
        {
            anim.Play("Attack");
            Vector3 bulletPos = transform.position;
            bulletPos.y += 0.6f;
            Vector3 direction = Vector3.Normalize(objective.transform.position - transform.position);
            GameManager.Instance.GenerateBullet(this.gameObject, bulletPos, direction);
        }
    }

    public override IEnumerator Die()
    {
        anim.Play("Die");
        if (EventManager.enemyDeathEvent != null)
            EventManager.enemyDeathEvent.CallEvent(this);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 0.5f);
        gameObject.SetActive(false);
    }

    public override void Mock()
    {

    }

    public override void Movement(Vector3 destination)
    {
        if(destination != null)
        {
            nav.SetDestination(destination);
        }
    }

}
