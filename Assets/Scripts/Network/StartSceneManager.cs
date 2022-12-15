using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviourPunCallbacks
{
    public void OnClickStart(){
        print("[ClickStart]");
        // PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        print("Connected! Joining Lobby...");
        SceneManager.LoadScene("LobbyScene");
    }
}
