using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

// using Text = TMPro.TextMeshProUGUI;

public class WaitingSceneManager : MonoBehaviourPunCallbacks
{
    public Text currRoomKeyText;
    public Text waitingPlayersText;
    public int maxPlayer = 4;

    void Start()
    {
        if(!PhotonNetwork.IsConnected){
            SceneManager.LoadScene("BeginScene");
        }
    }

    void Update()
    {
        if(PhotonNetwork.InRoom){
            currRoomKeyText.text = "Room Key: " + PhotonNetwork.CurrentRoom.Name;
            waitingPlayersText.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + maxPlayer + " are waiting...";
            if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayer)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    public void OnClickQuit(){
        print("[Click Quit]");
        if(PhotonNetwork.InRoom){
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom(){
        print("Left Room");
        SceneManager.LoadScene("LobbyScene");
    }
}
