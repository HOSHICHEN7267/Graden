using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerUIManager : MonoBehaviour
{
    GameManager _gm;

    // player info
    public List<GameObject> _alivePlayerUI; // the order is same as myPlayerList
    public List<GameObject> _deadPlayerUI; // the order is same as myPlayerList
    public int maxPlayer = 5;
    List<Player> myPlayerList;  // 0:       me
                                // 1 - 4:   other players

    // key info
    const int maxKey = 5;
    public List<GameObject> _keyUI; // 0:   no key
                                    // 1:   has key
                                    // 2:   boss key
    public Text _totalKeyText;
    int totalKey;

    public GameObject _keyStatus;

    // gravity info
    public List<GameObject> _gravityUI; // 0:   not change
                                        // 1:   changed
    
    // panels
    public GameObject _debuffPanel;
    public GameObject _deadPanel;

    // pv
    PhotonView _pv;

    void Start()
    {
        _gm = GameObject.FindObjectOfType<GameManager>();
        InitUI();
    }

    void Update()
    {
        
    }

    public void GiveKey(){  // give key into center
        ++totalKey;
        _keyUI[0].SetActive(true);
        _keyUI[1].SetActive(false);
        _keyUI[2].SetActive(false);
        _totalKeyText.text = totalKey.ToString() + "  /  " + maxKey.ToString();
    }

    public void GetKey(){  // get key
        _keyUI[0].SetActive(false);
        _keyUI[1].SetActive(true);
        _keyUI[2].SetActive(false);
    }

    public void PlayerDie(Player deadPlayer){
        int deadIndex = myPlayerList.FindIndex(x => x == deadPlayer);
        _alivePlayerUI[deadIndex].SetActive(false);
        _deadPlayerUI[deadIndex].SetActive(true);
        _debuffPanel.SetActive(false);
        _deadPanel.SetActive(true);
    }

    public void GravityChange(){
        _gravityUI[0].SetActive(!_gravityUI[0].activeSelf);
        _gravityUI[1].SetActive(!_gravityUI[1].activeSelf);
    }

    public void EnterDebuffArea(){
        _debuffPanel.SetActive(true);
    }

    public void LeaveDebuffArea(){
        _debuffPanel.SetActive(false);
    }

    public void ChangeToBossKey()
    {
        _keyUI[0].SetActive(false);
        _keyUI[1].SetActive(false);
        _keyUI[2].SetActive(true);
    }

    public void HideKeyStatus(){
        _keyStatus.SetActive(false);
    }

    void InitUI(){
        InitMyPlayerList();
        for(int i=0; i < maxPlayer; ++i){
            _alivePlayerUI[i].SetActive(true);
            _deadPlayerUI[i].SetActive(false);
        }
        if(_pv.IsMine){
            _keyUI[0].SetActive(true);
            _keyUI[1].SetActive(false);
            _keyUI[2].SetActive(false);
        }
        totalKey = 0;
        _totalKeyText.text = totalKey.ToString() + "  /  " + maxKey.ToString();
        _debuffPanel.SetActive(false);
        _deadPanel.SetActive(false);
    }

    void InitMyPlayerList(){
        // myPlayerList[0] = PhotonNetwork.LocalPlayer;
        // for(int i=1; i < maxPlayer; ++i){
        //     myPlayerList[i] = PhotonNetwork.PlayerListOthers[i-1];
        // }
    }
}
