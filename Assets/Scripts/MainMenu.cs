using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;
    public GameObject modeSelect;
    public GameObject JoinLobby;
    public GameObject multiplayerButton;

    public static bool onlineActive = false;

    void Start()
    {
        credits.SetActive(false);
        modeSelect.SetActive(false);
        JoinLobby.SetActive(false);
    }
    void Update()
    {
        Input();
    }

    public void Hola()
    {
        Debug.Log("Hola");
    }
    public void PlaySingleGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Credits()
    {
        credits.SetActive(!credits.activeInHierarchy);
    }
    public void ModeSelect()
    {
        Debug.Log("Online is " + onlineActive);
        modeSelect.SetActive(true);
        JoinLobby.SetActive(false);
        if (!onlineActive)
            multiplayerButton.SetActive(false);
        else
            multiplayerButton.SetActive(true);

    }
    public void JoinToLobby()
    {
        JoinLobby.SetActive(true);
    }


    public void Input()
    {
        Gamepad controller = Gamepad.current;

        if (controller == null) return;

        if(controller.startButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Game");
        }

        if (controller.selectButton.wasPressedThisFrame)
        {
            credits.SetActive(!credits.activeInHierarchy);
        }
    }
}
