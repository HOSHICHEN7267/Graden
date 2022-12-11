using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    // player info
    public List<GameObject> _alivePlayerUI; // the order is same as myPlayerList
    public List<GameObject> _deadPlayerUI; // the order is same as myPlayerList
    public int maxPlayer = 5;
    List<Player> myPlayerList;  // 0:       me
                                // 1 - 4:   other players

    // key info
    const int maxKey = 5;
    public List<GameObject> keyUI;  // 0:   no key
                                    // 1:   has key
    public Text _totalKey;
    int totalKey;

    // timer
    public Text _timer;
    public int timer_totalSec;
    int timer_min = 0;
    int timer_sec = 0;

    void Start()
    {
        InitUI();
        StartCoroutine(CountDown());
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
        _alivePlayerUI[deadIndex].SetActive(false);
        _deadPlayerUI[deadIndex].SetActive(true);
    }

    void InitUI(){
        InitMyPlayerList();
        for(int i=0; i < maxPlayer; ++i){
            _alivePlayerUI[i].SetActive(true);
            _deadPlayerUI[i].SetActive(false);
        }
        keyUI[0].SetActive(true);
        keyUI[1].SetActive(false);
        totalKey = 0;
        timer_min = timer_totalSec / 60;
        timer_sec = timer_totalSec % 60;
    }

    void InitMyPlayerList(){
        // myPlayerList[0] = PhotonNetwork.LocalPlayer;
        // for(int i=1; i < maxPlayer; ++i){
        //     myPlayerList[i] = PhotonNetwork.PlayerListOthers[i-1];
        // }
    }

    IEnumerator CountDown(){
        _timer.text = string.Format("{0} : {1}", timer_min.ToString("00"), timer_sec.ToString("00"));
        while(timer_totalSec > 0){
            yield return new WaitForSeconds(1);
            --timer_totalSec;
            --timer_sec;

            if(timer_sec < 0 && timer_min > 0){
                --timer_min;
                timer_sec = 59;
            }
            _timer.text = string.Format("{0} : {1}", timer_min.ToString("00"), timer_sec.ToString("00"));
        }
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
    }
}
