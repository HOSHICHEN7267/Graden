using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviourPunCallbacks
{
    public void OnClickStory(){
        print("[Click Story]");
        SceneManager.LoadScene("StoryScene");
    }

    public void OnClickTutorial(){
        print("[Click Tutorial]");
        SceneManager.LoadScene("TutorialScene");
    }

    public void OnClickPlay(){
        print("[Click Play]");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnClickQuitGame(){
        print("[Click Quit Game]");
        Application.Quit();
    }
    
    public override void OnConnectedToMaster(){
        print("Connected! Joining Lobby...");
        SceneManager.LoadScene("LobbyScene");
    }
}
