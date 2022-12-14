using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

using Text = TMPro.TextMeshProUGUI;

public class WaitingSceneManager : MonoBehaviourPunCallbacks
{
    public Text currRoomKeyText;
    public Text waitingPlayersText;
    public int maxPlayerCount = 5;

    void Start()
    {
        if(PhotonNetwork.IsConnected == false){
            SceneManager.LoadScene("StartScene");
        }
    }

    void Update()
    {
        if(PhotonNetwork.InRoom){
            currRoomKeyText.text = "Room Key: " + PhotonNetwork.CurrentRoom.Name;
            waitingPlayersText.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + maxPlayerCount + " are waiting...";
            if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayerCount){
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    public void OnClickQuit(){
        print("[ ClickQuit ]");
        if(PhotonNetwork.InRoom){
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom(){
        print("Left Room");
        SceneManager.LoadScene("LobbyScene");
    }
}
