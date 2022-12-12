using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Text = TMPro.TextMeshProUGUI;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    InputField inputRoomKey;

    void Start()
    {
        if(PhotonNetwork.IsConnected == false){
            SceneManager.LoadScene("StartScene");
        }
        else if(PhotonNetwork.CurrentLobby == null){
            PhotonNetwork.JoinLobby();
        }
    }

    // **********
    // Lobby
    // 

    public override void OnJoinedLobby(){
        print("Joined Lobby.");
    }

    // **********
    // Room
    // 

    public string GetRoomKey(){
        string roomKey = "";
        for(int i = 0; i < 3; ++i){
            int num = Random.Range(0, 10);
            roomKey += num.ToString();
        }
        return roomKey;
    }

    public void OnClickCreateRoom(){
        print("[ ClickCreate ]");
        while(!PhotonNetwork.CreateRoom(GetRoomKey())){
        }
    }

    public void OnClickJoinRoom(){
        print("[ ClickJoin ]");
        while(!PhotonNetwork.JoinRoom(inputRoomKey.text)){
            print("Invalid roomKey.");
        }
    }

    public override void OnJoinedRoom(){
        print("Joined Room " + PhotonNetwork.CurrentRoom.Name + ".");
        SceneManager.LoadScene("WaitingScene");
    }
}
