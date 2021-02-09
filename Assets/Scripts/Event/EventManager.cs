using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager Instance { get { return _instance; } }
    void Awake()
    {
        if (_instance == null)
        {

            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            //Rest of your Awake code
        }
        else
        {
            Destroy(this);
        }
    }

    public static ShipDamageEvent shipDamageEvent = new ShipDamageEvent();

    public static ShipDestroyEvent shipDestroyEvent = new ShipDestroyEvent();

    public static PlayerDeathEvent playerDeathEvent = new PlayerDeathEvent();

    public static PowerUpDespawnEvent powerUpDespawnEvent = new PowerUpDespawnEvent();

    public static PowerUpPickUpEvent powerUpPickUpEvent = new PowerUpPickUpEvent();

    public static EnemyDeathEvent enemyDeathEvent = new EnemyDeathEvent();


}
