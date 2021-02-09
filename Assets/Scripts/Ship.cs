using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IDamageable
{
    public float health = 1000;

    public bool isDestroyed = false;

    void Repair()
    {

    }

    void DestroyShip()
    {
        isDestroyed = true;
        if (EventManager.shipDestroyEvent != null)
            EventManager.shipDestroyEvent.CallEvent(this);
        gameObject.SetActive(false);
    }

    public void Damage(GameObject damager,float damage)
    {
        //Ignora el daño si es un jugador
        if (damager.tag == "Player")
            return;
        health -= damage;
        if (health <= 0)
            DestroyShip();
        if (EventManager.shipDamageEvent != null)
            EventManager.shipDamageEvent.CallEvent(this);
    }

    public bool IsAlive()
    {
        return !isDestroyed;
    }
}
