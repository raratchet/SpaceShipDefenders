using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float damage;
    public GameObject shooter;
    public float lifespan;
    private float spawnTimeStamp;

    void OnEnable()
    {
        spawnTimeStamp = Time.time;
    }

    void OnDisable()
    {
        transform.position = new Vector3(0,100,0);
        shooter = null;
    }

    void FixedUpdate()
    {
        Movement();
        CheckIfAlive();
    }

    private Vector3 movement = new Vector3();
    void Movement()
    {
        movement.x = transform.position.x + direction.x * speed * Time.deltaTime;
        movement.y = transform.position.y;
        movement.z = transform.position.z + direction.z * speed * Time.deltaTime;

        transform.position = movement;
    }

    void CheckIfAlive()
    {
        if((spawnTimeStamp + lifespan)  < Time.time)
        {
            this.gameObject.SetActive(false);
        }
    }

   void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag != shooter.tag)
        {
            foreach(var comp in col.GetComponents<IDamageable>())
            {
                IDamageable damagable = comp;
                if (!damagable.IsAlive()) 
                    return;
                damagable.Damage(shooter,damage);
            }
            this.gameObject.SetActive(false);
        }
    }

}
