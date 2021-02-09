using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;
    void Start()
    {
        credits.SetActive(false);
    }
    void Update()
    {
        Input();
    }
    public void Input()
    {
        Gamepad controller = Gamepad.current;
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
