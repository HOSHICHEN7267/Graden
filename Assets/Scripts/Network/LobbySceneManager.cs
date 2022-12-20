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
        if(!PhotonNetwork.IsConnected){
            SceneManager.LoadScene("StartScene");
        }
        else if(PhotonNetwork.CurrentLobby == null){
            while(!PhotonNetwork.JoinLobby()){
            }
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
        print("[ClickCreate]");
        if(!PhotonNetwork.CreateRoom(GetRoomKey())){
            print("Failed to create room.");
        }
    }

    public void OnClickJoinRoom(){
        print("[ClickJoin]");
        print("input key \"" + inputRoomKey.text + "\".");
        if(!PhotonNetwork.JoinRoom(inputRoomKey.text)){
            print("Failed to join room.");
        }
    }

    public override void OnCreateRoomFailed (short returnCode, string message){
        print("error " + ((int)returnCode).ToString() + ": " + message);
    }

    public override void OnJoinedRoom(){
        print("Joined Room " + PhotonNetwork.CurrentRoom.Name + ".");
        SceneManager.LoadScene("WaitingScene");
    }
    public override void OnJoinRoomFailed(short returnCode, string message) {
        print("error " + ((int)returnCode).ToString() + ": " + message);
    }
}
