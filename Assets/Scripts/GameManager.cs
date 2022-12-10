using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    int maxPlayer = 5;
    List<Player> myPlayerList;  // 0:       me
                                // 1 - 4:   other players
    public List<GameObject> alivePlayerUI; // the order is same as myPlayerList
    public List<GameObject> deadPlayerUI; // the order is same as myPlayerList
    public List<GameObject> keyUI;  // 0:   no key
                                    // 1:   has key
    public Text _totalKey;
    int totalKey;
    int maxKey = 5;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void GetKey(){
        ++totalKey;
        keyUI[0].SetActive(false);
        keyUI[1].SetActive(true);
        _totalKey.text = totalKey.ToString() + "  /  " + maxKey.ToString();
    }

    public void PutKey(){
        keyUI[0].SetActive(true);
        keyUI[1].SetActive(false);
    }

    public void PlayerDie(Player deadPlayer){
        int deadIndex = myPlayerList.FindIndex(x => x == deadPlayer);
        alivePlayerUI[deadIndex].SetActive(false);
        deadPlayerUI[deadIndex].SetActive(true);
    }

    void InitUI(){
        InitMyPlayerList();
        for(int i=0; i < maxPlayer; ++i){
            alivePlayerUI[i].SetActive(true);
            deadPlayerUI[i].SetActive(false);
        }
        keyUI[0].SetActive(true);
        keyUI[1].SetActive(false);
        totalKey = 0;
    }

    void InitMyPlayerList(){
        myPlayerList[0] = PhotonNetwork.LocalPlayer;
        for(int i=1; i < maxPlayer; ++i){
            myPlayerList[i] = PhotonNetwork.PlayerListOthers[i-1];
        }
    }
}
