using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Trying to connect to network");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Conected Joining to Lobby");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Online is now active");
        MainMenu.onlineActive = true;
    }
}
