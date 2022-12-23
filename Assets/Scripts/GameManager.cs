using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    PhotonView _pv;

    // player info
    public GameObject[] _identityUI;    // 0:   boss
                                        // 1:   clone
    public GameObject _alivePlayerUI; // the order is same as myPlayerList
    public GameObject _deadPlayerUI; // the order is same as myPlayerList
    public int maxPlayer = 4;
    int bossIndex;
    int myIndex;
    List<Player> myPlayerList = new List<Player>();     // 0:       me
                                                        // 1 - 4:   other players
    Vector3[] posi = {  new Vector3(0, 0.5f, -52f),
                        new Vector3(-52f, 0.5f, 0),
                        new Vector3(52f, 0.5f, 0),
                        new Vector3(-32f, 0.5f, 60f) };
    string[] playerName = { "#050300",
                            "#050500",
                            "#060000",
                            "#050715" };
    string bossName = "Vargity";

    // key info
    const int MAX_KEY = 4;
    public List<GameObject> _keyUI; // 0:   no key
                                    // 1:   has key
                                    // 2:   boss key
    public Text _totalKeyText;
    int totalKey;

    // gravity info
    public List<GameObject> _gravityUI; // 0:   not change
                                        // 1:   changed
    
    // panels
    public GameObject _debuffPanel;
    public GameObject _deadPanel;
    public GameObject[] _gameOverPanel; // 0:   boss win
                                        // 1:   clones win
                                        // 2:   tie

    // mini map
    public GameObject _miniMap; // 0:   center lab
                                      // 1:   lab1
                                      // 2:   lab2
                                      // .... 
                                      // 5:   lab5
    int[][] MINIMAP_POSI_X = {  new int[] {-16, 16},
                                new int[] {-92, -48},
                                new int[] {-88, -28},
                                new int[] {28, 88},
                                new int[] {48, 92},
                                new int[] {-20, 20},
                                new int[] {-28, 28},
                                new int[] {-77, -63},
                                new int[] {63, 77},
                                new int[] {-28, 28},
                                new int[] {-28, 28},
                                new int[] {-48, -16},
                                new int[] {16, 48},
                                new int[] {-94, -74},
                                new int[] {74, 94},
                                new int[] {-94, -20},
                                new int[] {20, 94}};
    int[][] MINIMAP_POSI_Z = {  new int[] {-16, 16},
                                new int[] {-20, 20},
                                new int[] {48, 72},
                                new int[] {48, 72},
                                new int[] {-20, 20},
                                new int[] {-92, -48},
                                new int[] {36, 72},
                                new int[] {20, 48},
                                new int[] {20, 48},
                                new int[] {14, 48},
                                new int[] {-48, -14},
                                new int[] {-48, 48},
                                new int[] {-48, 48},
                                new int[] {-78, -20},
                                new int[] {-70, -20},
                                new int[] {-74, -66},
                                new int[] {-70, -54}};

    // timer
    public Text _timerText;
    public int timer_totalSec;
    int timer_min = 0;
    int timer_sec = 0;

    // camera
    public GameObject _freeLookBoss;
    public GameObject _freeLookClone;

    // key generate
    // public GameObject key1,key2,key3,key4,key5,key6,key7,key8,key9,key10;
    int Ran_Lab1,Ran_Lab2,Ran_Lab3,Ran_Lab4,Ran_Lab5;
    int Lab1Extra, Lab2Extra, Lab3Extra,Lab4Extra,Lab5Extra;

    //用作取得玩家數量的，決定鑰匙生成與否
    public int MaxGamePlayer;

    void Start()
    {
        if(!PhotonNetwork.IsConnected){
            SceneManager.LoadScene("MenuScene");
        }
        else if(PhotonNetwork.CurrentRoom == null){
            SceneManager.LoadScene("LobbyScene");
        }
        _pv = this.gameObject.GetComponent<PhotonView>();
        if(PhotonNetwork.IsMasterClient){
            PickBoss();
            _pv.RPC("RPC_Init", RpcTarget.All, bossIndex);
        }
    }

    public List<int> generateRandom (int Length)
        {
            int Rand;
            List<int> list = new List<int>();
            list = new List<int>();
    
            for (int j = 0; j < Length; j++)
            {
                Rand = Random.Range(0,5);
    
                while (list.Contains(Rand))
                {
                    Rand = Random.Range(0,5);
                }
    
                list.Add(Rand);
            }

            return list;
    
        }

    public void RandomGenerateKey(){
        int maxKeyAmount = (MaxGamePlayer - 1)*2;

        List<int> keyList = generateRandom(maxKeyAmount-5);
        
        for (int i = 0; i < keyList.Count; i++){
            if (keyList[i] == 0){
                Lab1Extra += 1;
            }
            else if(keyList[i] == 1){
                Lab2Extra += 1;
            }
            else if(keyList[i] == 2){
                Lab3Extra += 1;
            }
            else if(keyList[i] == 3){
                Lab4Extra += 1;
            }
            else if(keyList[i] == 4){
                Lab5Extra += 1;
            }

        }
        
        // Lab 1
        Vector3[] positions1 = new Vector3[5];
        positions1[0] = new Vector3(-84.225f,0.88f,7.35f);
        positions1[1] = new Vector3(-64.97f,0.88f,-10.23f);
        positions1[2] = new Vector3(-64.77f,7.32f,-5.7f);
        positions1[3] = new Vector3(-64.8f,4.83f,-11.43f);
        positions1[4] = new Vector3(-75.41f,0.72f,-17.73f);
        Ran_Lab1 = Random.Range(0,5);
        if (Lab1Extra != 0){
            List<int> SecondRandom = new List<int>();
            for (int i = 0; i < 5; i++){
                SecondRandom.Add(i);
            }
            SecondRandom.Remove(Ran_Lab1);
            int SecondRandomNum = Random.Range(0,4);
            PhotonNetwork.Instantiate("KeyFragment", positions1[SecondRandom[SecondRandomNum]], Quaternion.identity);
            // key6.SetActive(true);
            // key6.transform.localPosition = positions1[SecondRandom[SecondRandomNum]];
        }
        PhotonNetwork.Instantiate("KeyFragment", positions1[Ran_Lab1], Quaternion.identity);
        // key1.transform.localPosition = positions1[Ran_Lab1];
        // Lab 2
        Vector3[] positions2 = new Vector3[5];
        positions2[0] = new Vector3(-45.802f,4.83f,49.81f);
        positions2[1] = new Vector3(-47.01f,4.83f,41.07f);
        positions2[2] = new Vector3(-48.08f,-2.151f,42.33f);
        positions2[3] = new Vector3(-48.08f,0.61f,52.66f);
        positions2[4] = new Vector3(-38.412f,-1.835f,40.73f);
        Ran_Lab2 = Random.Range(0,5);
        if (Lab2Extra != 0){
            List<int> SecondRandom = new List<int>();
            for (int i = 0; i < 5; i++){
                SecondRandom.Add(i);
            }
            SecondRandom.Remove(Ran_Lab2);
            int SecondRandomNum = Random.Range(0,4);
            PhotonNetwork.Instantiate("KeyFragment", positions2[SecondRandom[SecondRandomNum]], Quaternion.identity);
            // key7.SetActive(true);
            // key7.transform.localPosition = positions2[SecondRandom[SecondRandomNum]];
        }
        PhotonNetwork.Instantiate("KeyFragment", positions2[Ran_Lab1], Quaternion.identity);
        // key2.transform.localPosition = positions2[Ran_Lab2];
        // Lab 3
        Vector3[] positions3 = new Vector3[5];
        positions3[0] = new Vector3(67.94f,1.76f,49f);
        positions3[1] = new Vector3(80.74f,-2.151f,41.24f);
        positions3[2] = new Vector3(78.779f,1.89f,49.101f);
        positions3[3] = new Vector3(88.946f,1.692f,50.31f);
        positions3[4] = new Vector3(104.826f,1.872f,50.848f);
        Ran_Lab3 = Random.Range(0,5);
        if (Lab3Extra != 0){
            List<int> SecondRandom = new List<int>();
            for (int i = 0; i < 5; i++){
                SecondRandom.Add(i);
            }
            SecondRandom.Remove(Ran_Lab3);
            int SecondRandomNum = Random.Range(0,4);
            PhotonNetwork.Instantiate("KeyFragment", positions3[SecondRandom[SecondRandomNum]], Quaternion.identity);
            // key8.SetActive(true);
            // key8.transform.localPosition = positions3[SecondRandom[SecondRandomNum]];
        }
        PhotonNetwork.Instantiate("KeyFragment", positions3[Ran_Lab1], Quaternion.identity);
        // key3.transform.localPosition = positions3[Ran_Lab3];
        // Lab 4
        Vector3[] positions4 = new Vector3[5];
        positions4[0] = new Vector3(93.14f,1.84f,-11.46f);
        positions4[1] = new Vector3(95.69f,-2.151f,1.16f);
        positions4[2] = new Vector3(108.103f,-2.13f,42.33f);
        positions4[3] = new Vector3(94.101f,1.946f,6.117f);
        positions4[4] = new Vector3(72.88f,4.44f,-10.69f);
        Ran_Lab4 = Random.Range(0,5);
        if (Lab4Extra != 0){
            List<int> SecondRandom = new List<int>();
            for (int i = 0; i < 5; i++){
                SecondRandom.Add(i);
            }
            SecondRandom.Remove(Ran_Lab4);
            int SecondRandomNum = Random.Range(0,4);
            PhotonNetwork.Instantiate("KeyFragment", positions4[SecondRandom[SecondRandomNum]], Quaternion.identity);
            // key9.SetActive(true);
            // key9.transform.localPosition = positions4[SecondRandom[SecondRandomNum]];
        }
        PhotonNetwork.Instantiate("KeyFragment", positions4[Ran_Lab1], Quaternion.identity);
        // key4.transform.localPosition = positions4[Ran_Lab4];
        // Lab 5
        Vector3[] positions5 = new Vector3[5];
        positions5[0] = new Vector3(3f,3.3f,-71.6f);
        positions5[1] = new Vector3(17.45f,4.83f,-76.78f);
        positions5[2] = new Vector3(11.05f,4.83f,-57.4f);
        positions5[3] = new Vector3(-11.07f,0.88f,-58.58f);
        positions5[4] = new Vector3(-14.7f,0.88f,-89.87f);
        Ran_Lab5 = Random.Range(0,5);
        if (Lab5Extra != 0){
            List<int> SecondRandom = new List<int>();
            for (int i = 0; i < 5; i++){
                SecondRandom.Add(i);
            }
            SecondRandom.Remove(Ran_Lab5);
            int SecondRandomNum = Random.Range(0,4);
            PhotonNetwork.Instantiate("KeyFragment", positions5[SecondRandom[SecondRandomNum]], Quaternion.identity);
            // key10.SetActive(true);
            // key10.transform.localPosition = positions5[SecondRandom[SecondRandomNum]];
        }
        PhotonNetwork.Instantiate("KeyFragment", positions5[Ran_Lab1], Quaternion.identity);
        // key5.transform.localPosition = positions5[Ran_Lab5];
    }

    void PickBoss(){
        bossIndex = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        print("Boss is " + bossIndex.ToString());
    }

    [PunRPC]
    void RPC_Init(int _bossIndex){
        print("Initializing game...");
        // bossIndex
        bossIndex = _bossIndex;
        myIndex = 0;
        // myIndex
        for(; myIndex < maxPlayer; ++myIndex){
            if(PhotonNetwork.PlayerList[myIndex] == PhotonNetwork.LocalPlayer){
                break;
            }
        }
        print("myIndex = " + myIndex.ToString());
        // init UI
        InitUI();
        print("my name is " + myPlayerList[myIndex].NickName);
        // instantiate
        if(isBoss(myPlayerList[myIndex].NickName)){
            PhotonNetwork.Instantiate("Boss_" + myIndex.ToString(), posi[myIndex], Quaternion.identity);
            Instantiate(_freeLookBoss, posi[myIndex], Quaternion.identity);
            RandomGenerateKey();
        }
        else{
            PhotonNetwork.Instantiate("Clone_" + myIndex.ToString(), posi[myIndex], Quaternion.identity);
            Instantiate(_freeLookClone, posi[myIndex], Quaternion.identity);
        }
        StartCoroutine(CountDown());
        StartCoroutine(ShowIdentity());
        print("Game initialized.");
    }

    void InitUI(){
        InitMyPlayerList();
        // i: myPlayerList
        // j: j-th player in PlayerUI
        // k: the UI you wanna modify
        for(int i = 0, j = 1, k; i < maxPlayer; ++i){
            if(i == myIndex){
                k = 0;
                if(isBoss(myPlayerList[i].NickName)){
                    PhotonNetwork.LocalPlayer.NickName = bossName;
                }
                else{
                    PhotonNetwork.LocalPlayer.NickName = playerName[i];
                }
            }
            else{
                k = j;
                ++j;
            }
            _alivePlayerUI.transform.GetChild(k).gameObject.SetActive(true);
            _alivePlayerUI.transform.GetChild(k).transform.GetChild(1).GetComponent<Text>().text = myPlayerList[i].NickName;
            _deadPlayerUI.transform.GetChild(k).gameObject.SetActive(false);
            _deadPlayerUI.transform.GetChild(k).transform.GetChild(1).GetComponent<Text>().text = myPlayerList[i].NickName;
            print("player " + i + ": " + myPlayerList[i].NickName + " in k = " + k.ToString());
        }
        if(_pv.IsMine){
            _keyUI[0].SetActive(true);
            _keyUI[1].SetActive(false);
            _keyUI[2].SetActive(false);
        }
        totalKey = 0;
        _totalKeyText.text = totalKey.ToString() + "  /  " + MAX_KEY.ToString();
        _debuffPanel.SetActive(false);
        _deadPanel.SetActive(false);
    }

    void InitMyPlayerList(){
        for(int i = 0; i < maxPlayer; ++i){
            myPlayerList.Add(PhotonNetwork.PlayerList[i]);
            if(i == bossIndex){
                myPlayerList[i].NickName = bossName;
            }
            else{
                myPlayerList[i].NickName = playerName[i];
            }
            print("myPlayerList[" + i + "] = " + myPlayerList[i]);
        }
    }

    IEnumerator CountDown(){
        timer_min = timer_totalSec / 60;
        timer_sec = timer_totalSec % 60;
        _timerText.text = string.Format("{0} : {1}", timer_min.ToString("00"), timer_sec.ToString("00"));
        while(timer_totalSec > 0){
            yield return new WaitForSeconds(1);
            --timer_totalSec;
            --timer_sec;

            if(timer_sec < 0 && timer_min > 0){
                --timer_min;
                timer_sec = 59;
            }
            _timerText.text = string.Format("{0} : {1}", timer_min.ToString("00"), timer_sec.ToString("00"));
        }
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        Tie();
    }

    IEnumerator ShowIdentity(){
        if(isBoss(myPlayerList[myIndex].NickName)){
            _identityUI[0].SetActive(true);
            _identityUI[1].SetActive(false);
            yield return new WaitForSeconds(2);
            StartCoroutine(FadeOutIdentity(0));
        }
        else{
            _identityUI[0].SetActive(false);
            _identityUI[1].SetActive(true);
            yield return new WaitForSeconds(2);
            StartCoroutine(FadeOutIdentity(1));
        }
    }

    IEnumerator FadeOutIdentity(int index){
        float x = _identityUI[index].transform.localScale.x;
        float y = _identityUI[index].transform.localScale.y;
        float z = _identityUI[index].transform.localScale.z;
        float rate = 0.9f;
        while(y > 0.01f){
            y *= rate;
            rate *= rate;
            _identityUI[index].transform.localScale = new Vector3(x, y, z);
            yield return new WaitForSeconds(0.01f);
        }
        _identityUI[index].SetActive(false);
    }

    bool isBoss(string playerName){
        return playerName == bossName;
    }

    public void BossWin(){
        StartCoroutine(EndGame(0));
    }

    public void CloneWin(){
        StartCoroutine(EndGame(1));
    }

    public void Tie(){
        StartCoroutine(EndGame(2));
    }

    IEnumerator EndGame(int gameOverCode){
        print("Ending game...");
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(FadeInGameOverPanel(gameOverCode));
    }

    IEnumerator FadeInGameOverPanel(int gameOverCode){
        _gameOverPanel[gameOverCode].SetActive(true);
        float originX = _gameOverPanel[gameOverCode].transform.localScale.x;
        float originY = _gameOverPanel[gameOverCode].transform.localScale.y;
        float originZ = _gameOverPanel[gameOverCode].transform.localScale.z;
        float y = 0;
        float unit = originY/5f;
        _gameOverPanel[gameOverCode].transform.localScale = new Vector3(originX, y, originZ);
        while(y < originY){
            y += unit;
            _gameOverPanel[gameOverCode].transform.localScale = new Vector3(originX, y, originZ);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(0.8f);
        for(int i = 1; i < 3; ++i){
            _gameOverPanel[gameOverCode].transform.GetChild(i).gameObject.SetActive(true);
            originX = _gameOverPanel[gameOverCode].transform.GetChild(i).localScale.x;
            originY = _gameOverPanel[gameOverCode].transform.GetChild(i).localScale.y;
            originZ = _gameOverPanel[gameOverCode].transform.GetChild(i).localScale.z;
            y = 0;
            unit = originY/5f;
            _gameOverPanel[gameOverCode].transform.GetChild(i).localScale = new Vector3(originX, y, originZ);
            while(y < originY){
                y += unit;
                _gameOverPanel[gameOverCode].transform.GetChild(i).localScale = new Vector3(originX, y, originZ);
                yield return new WaitForSecondsRealtime(0.01f);
            }
            yield return new WaitForSecondsRealtime(1.5f);
        }
    }

    public void OnClickReturnToLobby(){
        print("[ClickReturnToLobby]");
        if(PhotonNetwork.InRoom){
            Time.timeScale = 1;
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom(){
        SceneManager.LoadScene("LobbyScene");
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
        _totalKeyText.text = totalKey.ToString() + "  /  " + MAX_KEY.ToString();
        if(totalKey == MAX_KEY){
            CloneWin();
        }
    }

    public void GetKey(){  // get key
        _keyUI[0].SetActive(false);
        _keyUI[1].SetActive(true);
        _keyUI[2].SetActive(false);
    }

    public void PlayerDie(Player deadPlayer){
        if(deadPlayer == PhotonNetwork.LocalPlayer){
            StartCoroutine(FadeInDeadPanel());
        }
        int deadIndex = myPlayerList.FindIndex(x => x.NickName == deadPlayer.NickName);
        string deadName = myPlayerList[deadIndex].NickName;
        _pv.RPC("RPC_SyncPlayer", RpcTarget.All, deadName);
    }

    [PunRPC]
    void RPC_SyncPlayer(string deadName){
        print("dead player name: " + deadName);
        List<string> myPlayerUI = new List<string>();
        myPlayerUI.Add(playerName[myIndex]);
        for(int i = 0; i < maxPlayer; ++i){
            if(i == myIndex){
                continue;
            }
            myPlayerUI.Add(playerName[i]);
        }
        int deadIndex = myPlayerUI.FindIndex(x => x == deadName);
        print(PhotonNetwork.LocalPlayer + ": deadIndex = " + deadIndex);
        _alivePlayerUI.transform.GetChild(deadIndex).gameObject.SetActive(false);
        _deadPlayerUI.transform.GetChild(deadIndex).gameObject.SetActive(true);
        if(isAllDead()){
            BossWin();
        }
        else if(deadName == bossName){
            CloneWin();
        }
    }

    IEnumerator FadeInDeadPanel(){
        // panel
        _deadPanel.SetActive(true);
        float originR = _deadPanel.GetComponent<Image>().color.r;
        float originB = _deadPanel.GetComponent<Image>().color.b;
        float originG = _deadPanel.GetComponent<Image>().color.g;
        float originAlpha = _deadPanel.GetComponent<Image>().color.a;
        float alpha = 0;
        float unit = originAlpha/5f;
        _deadPanel.GetComponent<Image>().color = new Color(originR, originB, originG, alpha);
        while(alpha < originAlpha){
            alpha += unit;
            _deadPanel.GetComponent<Image>().color = new Color(originR, originB, originG, alpha);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        // title
        _deadPanel.transform.GetChild(0).gameObject.SetActive(true);
        originR = _deadPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color.r;
        originB = _deadPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color.b;
        originG = _deadPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color.g;
        originAlpha = _deadPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color.a;
        alpha = 0;
        unit = originAlpha/5f;
        _deadPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color = new Color(originR, originB, originG, alpha);
        while(alpha < originAlpha){
            alpha += unit;
            _deadPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color = new Color(originR, originB, originG, alpha);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(1.5f);
        // buttons
        _deadPanel.transform.GetChild(1).gameObject.SetActive(true);
        float originX = _deadPanel.transform.GetChild(1).localScale.x;
        float originY = _deadPanel.transform.GetChild(1).localScale.y;
        float originZ = _deadPanel.transform.GetChild(1).localScale.z;
        float y = 0;
        unit = originY/5f;
        _deadPanel.transform.GetChild(1).localScale = new Vector3(originX, y, originZ);
        while(y < originY){
            y += unit;
            _deadPanel.transform.GetChild(1).localScale = new Vector3(originX, y, originZ);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    bool isAllDead(){
        bool flag = false;
        for(int i = 0; i < maxPlayer; ++i){
            if(isBoss(myPlayerList[i].NickName)){
                continue;
            }
            flag |= _deadPlayerUI.transform.GetChild(i).gameObject.activeSelf;
        }
        return !flag;
    }

    public void GravityChange(){
        _gravityUI[0].SetActive(!_gravityUI[0].activeSelf);
        _gravityUI[1].SetActive(!_gravityUI[1].activeSelf);
    }

    // miniMap
    public int InRoom(float x, float z){
        resetMiniMap();
        int returnRoom = -1;
        for(int i = 0; i < MINIMAP_POSI_X.Length-2; ++i){
            if(i >= 13  && ((   MINIMAP_POSI_X[i][0] < x && x < MINIMAP_POSI_X[i][1]
                            &&  MINIMAP_POSI_Z[i][0] < z && z < MINIMAP_POSI_Z[i][1])
                        ||   (  MINIMAP_POSI_X[i+2][0] < x && x < MINIMAP_POSI_X[i+2][1]
                            &&  MINIMAP_POSI_Z[i+2][0] < z && z < MINIMAP_POSI_Z[i+2][1]))){
                    _miniMap.transform.GetChild(i+1).gameObject.SetActive(true);
                    returnRoom = i;
                }
            else if(    MINIMAP_POSI_X[i][0] < x && x < MINIMAP_POSI_X[i][1]
                    &&  MINIMAP_POSI_Z[i][0] < z && z < MINIMAP_POSI_Z[i][1]){
                _miniMap.transform.GetChild(i+1).gameObject.SetActive(true);
                returnRoom = i;
            }
        }
        return returnRoom;
    }

    void resetMiniMap(){
        _miniMap.transform.GetChild(0).gameObject.SetActive(true);
        for(int i = 1; i < MINIMAP_POSI_X.Length-1; ++i){
            _miniMap.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    
    public void SlowSpeed()
    {
        _debuffPanel.SetActive(true);
    }

    public void NormalSpeed()
    {
        _debuffPanel.SetActive(false);
    }

    public void ChangeToBossKey()
    {
        _keyUI[0].SetActive(false);
        _keyUI[1].SetActive(false);
        _keyUI[2].SetActive(true);
    }
    
    public override void OnPlayerLeftRoom (Player otherPlayer){
        if(_pv.IsMine){
            PlayerDie(otherPlayer);
        }
    }
}
