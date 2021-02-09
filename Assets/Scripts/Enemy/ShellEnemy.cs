using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ShellEnemy : Enemy
{
    public GameObject currentObjective;
    public float detectionRange;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        objective = GameObject.FindGameObjectWithTag("Ship");
        currentObjective = objective;
        nav.stoppingDistance = range;
        EventManager.playerDeathEvent.myEvent += OnPlayerKillEvent;
    }

    void FixedUpdate()
    {
            ChangeObjective();
    }

    void Update()
    {
        if (GameManager.Instance.gameOver)
        {
            nav.isStopped = true;
            return;
        }
        if (range >= Vector3.Distance(transform.position, currentObjective.transform.position))
        {
            if (lastAttack + attackCooldown < Time.time)
            {
                Attack();
                lastAttack = Time.time;
            }
        }
    }

    void OnEnable()
    {
        objective = GameObject.FindGameObjectWithTag("Ship");
        currentObjective = objective;
        Movement(currentObjective.transform.position);
        health = baseHealth;
        speed = baseSpeed;
        isDead = false;
    }

    public override void Attack()
    {
        if (!isDead)
        {
            IDamageable toAttack = (IDamageable)currentObjective.GetComponent<MonoBehaviour>();
            if(toAttack.IsAlive())
            {
                anim.Play("Attack");
                toAttack.Damage(this.gameObject, damage);
            }
        }
    }

    public void OnPlayerKillEvent()
    {
        anim.Play("Taunt");
        currentObjective = objective;
        Debug.Log("Ahora mi objetivo es " + currentObjective);
        Movement(currentObjective.transform.position);
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

    public void ChangeObjective()
    {
        foreach(var obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(detectionRange >= Vector3.Distance(transform.position, obj.transform.position))
            {
                IDamageable toDmg = (IDamageable)obj.GetComponent<MonoBehaviour>();
                if (toDmg.IsAlive())
                {
                    currentObjective = obj;
                    Movement(currentObjective.transform.position);
                }
            }
            else
            {
                if(currentObjective != objective)
                {
                    currentObjective = objective;
                    Movement(currentObjective.transform.position);
                }
            }
        }
    }

    public override void Movement(Vector3 destination)
    {
        nav.SetDestination(destination);
    }
}
