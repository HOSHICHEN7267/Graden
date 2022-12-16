using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerUIManager : MonoBehaviour
{
    GameManager _gm;
    PhotonView _pv;

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

    // mini map
    public List<GameObject> _miniMap; // 0:   center lab
                                      // 1:   lab1
                                      // 2:   lab2
                                      // .... 
                                      // 5:   lab5  

    void Start()
    {
        _gm = GameObject.FindObjectOfType<GameManager>();
        _pv = this.gameObject.GetComponent<PhotonView>();
        InitUI();
    }

    void Update()
    {
        
    }

    public void GiveKey(){  // give key into center
        ++totalKey;
        _pv.RPC("RPC_SyncKey", RpcTarget.All, totalKey);
        _keyUI[0].SetActive(true);
        _keyUI[1].SetActive(false);
        _keyUI[2].SetActive(false);
    }

    [PunRPC]
    void RPC_SyncKey(int num){
        totalKey = num;
        _totalKeyText.text = totalKey.ToString() + "  /  " + maxKey.ToString();
        if(totalKey == maxKey){
            _gm.CloneWin();
        }
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
        if(isAllDead()){
            _gm.BossWin();
        }
    }

    bool isAllDead(){
        bool flag = false;
        foreach(GameObject p in _deadPlayerUI){
            flag |= p.activeSelf;
        }
        return !flag;
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

    // miniMap
    public void InCorridor()
    {
        int i;
        for (i = 0; i < 6; i++)
        {
            _miniMap[i].SetActive(false);
        }
    }
    public void EnterCenterLab()
    {
        _miniMap[0].SetActive(true);
    }
    public void EnterLab1()
    {
        
        _miniMap[1].SetActive(true);
    }
    public void EnterLab2()
    {

        _miniMap[2].SetActive(true);
    }
    public void EnterLab3()
    {

        _miniMap[3].SetActive(true);
    }
    public void EnterLab4()
    {

        _miniMap[4].SetActive(true);
    }
    public void EnterLab5()
    {

        _miniMap[5].SetActive(true);
    }
    
    public void SlowSpeed()
    {
        _debuffPanel.SetActive(true);
    }

    public void NormalSpeed()
    {
        _debuffPanel.SetActive(false);
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
