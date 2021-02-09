using UnityEngine;
using UnityEngine.UI;
public class GameUI : MonoBehaviour
{
    public Slider playerH;
    public Slider shipH;
    public Text eLeft;
    public Text eAlive;
    public Text wave;
    public Text ammo;
    public GameObject GameOver;
    public Text survived;

    public Player player;
    public Ship ship;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<Ship>();
        playerH.maxValue = player.baseHealth;
        shipH.maxValue = ship.health;
        GameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        playerH.value = player.health;
        shipH.value = ship.health;
        eLeft.text = "Enemies Left " + (GameManager.Instance.enemiesLeft +  GameManager.Instance.enemiesAlive);
        eAlive.text = "Enemies Alive " + GameManager.Instance.enemiesAlive;
        wave.text = "Wave " + GameManager.Instance.wave;
        ammo.text = "Ammo " + player.ammo;
        if(GameManager.Instance.gameOver)
        {
            GameOver.SetActive(true);
            survived.text = "You survived " + (GameManager.Instance.wave - 1) + " waves";
        }
    }
}
