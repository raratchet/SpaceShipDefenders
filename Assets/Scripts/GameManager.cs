using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{

    BulletFactory bulletFactory = null;
    EnemyFactory enemyFactory = null;

    public int wave;
    public int enemiesLeft;
    public int enemiesAlive;
    public float waitSpawnTime;
    public int enemiesPerWave;
    public GameObject playerPrefab;
    public GameObject[] playerSpawners;

    public bool gameOver = false;
    public static bool isMultiplayer = false;
    public static bool isOnline = false;

    private List<Ship> activeShips = new List<Ship>();
    private List<Player> deathPlayers = new List<Player>();
    private int playerCount = 0;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
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

    void Start()
    {
        RegisterToEvents();
        bulletFactory = FindObjectOfType<BulletFactory>();
        enemyFactory = FindObjectOfType<EnemyFactory>();
        StartCoroutine(StartGame());
        foreach(var ship in GameObject.FindGameObjectsWithTag("Ship"))
        {
            activeShips.Push_back(ship.GetComponent<Ship>());
        }
        SpawnPlayer();

        Debug.Log("Multiplayer: " + isMultiplayer + "Online: " + isOnline);
    }

    void Update()
    {

    }

    void SpawnPlayer()
    {
        if(isMultiplayer && isOnline)
        {
            //Aqui con photon
            GameObject p = PhotonNetwork.Instantiate(playerPrefab.name, playerSpawners[playerCount].transform.position, Quaternion.identity);
            p.transform.parent = null;
        }
        else
        {
            GameObject p = Instantiate(playerPrefab, playerSpawners[playerCount].transform);
            p.transform.parent = null;
        }
        playerCount++;
    }

    void RegisterToEvents()
    {
        EventManager.shipDamageEvent.myEvent += OnShipDamageEvent;
        EventManager.shipDestroyEvent.myEvent += OnShipDestroyEvent;
        EventManager.enemyDeathEvent.myEvent += OnEnemyDeathEvent;
        EventManager.playerDeathEvent.myEvent += OnPlayerDeathEvent;
    }
    void StartWave()
    {
        wave++;
        Debug.Log("Inicia la oleada " + wave);
        enemiesLeft = wave * enemiesPerWave;
        //bulletFactory.maxBulletCount = enemiesLeft * 3;
        enemiesAlive = 0;
        InitialWaveSpawn();
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2);
        StartWave();
    }
    void InitialWaveSpawn()
    {
        int quantity = (enemiesLeft / 10) > 5 ? enemiesLeft / 10 : 5;
        for(int i = 0; i < quantity; i++)
        {
            enemyFactory.SpawnRandomEnemy(-1, -1);
        }
        enemiesLeft -= quantity;
        enemiesAlive += quantity;
    }
    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2);
        while(enemiesLeft > 0 && !gameOver)
        {
            if(enemiesAlive <= enemyFactory.maxEnemyCount)
            {
                enemyFactory.SpawnRandomEnemy(-1, -1);
                enemiesLeft--;
                enemiesAlive++;
            }
                yield return new WaitForSeconds(waitSpawnTime);
        }
    }
    void OnShipDamageEvent()
    {

    }
    void OnPlayerDeathEvent()
    {
        StartCoroutine(RevivePlayer((Player)EventManager.playerDeathEvent.sender));
    }
    void OnEnemyDeathEvent()
    {
        enemiesAlive--;
        if(enemiesAlive <= 0 && enemiesLeft <= 0)
        {
            StopAllCoroutines();
            StartWave();
        }
    }
    void OnPowerUpDespawnEvent()
    {

    }
    void OnPowerUpPickUpEvent()
    {

    }
    void OnShipDestroyEvent()
    {
        activeShips.Remove((Ship)EventManager.shipDestroyEvent.sender);
        gameOver = true;
        StartCoroutine(EndGame());
    }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }
    IEnumerator RevivePlayer(Player player)
    {
        yield return new WaitForSeconds(3f);
        player.Respawn();
    }
    public void GenerateBullet(GameObject shooter,Vector3 position, Vector3 direction)
    {
        bulletFactory.InstantiateBullet(shooter,position, direction);
    }
    public void GenerateBullet(GameObject shooter, Vector3 position, Vector3 direction, float speed)
    {
        bulletFactory.InstantiateBullet(shooter, position, direction, speed);
    }
    public void GenerateBullet(GameObject shooter, Vector3 position, Vector3 direction, float speed, float damage)
    {
        bulletFactory.InstantiateBullet(shooter, position, direction,speed, damage);
    }
}
