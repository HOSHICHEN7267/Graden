using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Text = TMPro.TextMeshProUGUI;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    InputField inputRoomKey;
    string roomForTest = "4t";

    void Start()
    {
        if(!PhotonNetwork.IsConnected){
            SceneManager.LoadScene("BeginScene");
        }
        else if(PhotonNetwork.CurrentLobby == null){
            PhotonNetwork.JoinLobby();
        }
    }
    
    // Lobby

    public override void OnJoinedLobby(){
        print("Joined Lobby.");
    }

    // Room

    public string GetRoomKey(){
        string roomKey = "";
        for(int i = 0; i < 3; ++i){
            int num = Random.Range(0, 10);
            roomKey += num.ToString();
        }
        return roomKey;
    }

    public void OnClickJoinRoom(){
        print("[Click Join]");
        print("input key \"" + inputRoomKey.text + "\".");
        RoomOptions roomOptions = new RoomOptions();
        if(inputRoomKey.text == roomForTest){
            PhotonNetwork.JoinOrCreateRoom(roomForTest, roomOptions, null);
        }
        else if(!PhotonNetwork.JoinRoom(inputRoomKey.text)){
            print("Failed to join room.");
        }
    }

    public void OnClickCreateRoom(){
        print("[Click Create]");
        if(!PhotonNetwork.CreateRoom(GetRoomKey())){
            print("Failed to create room.");
        }
    }

    public void OnClickQuit(){
        print("[Click Quit]");
        if(PhotonNetwork.IsConnected){
            PhotonNetwork.Disconnect();
        }
    }

    // override

    public override void OnJoinedRoom(){
        print("Joined Room " + PhotonNetwork.CurrentRoom.Name + ".");
        SceneManager.LoadScene("WaitingScene");
    }
    public override void OnJoinRoomFailed(short returnCode, string message) {
        print("error " + ((int)returnCode).ToString() + ": " + message);
    }

    public override void OnCreateRoomFailed (short returnCode, string message){
        print("error " + ((int)returnCode).ToString() + ": " + message);
    }

    public override void OnDisconnected(DisconnectCause cause){
        print("Disconnected.");
        SceneManager.LoadScene("MenuScene");
    }
}
