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
    Player bossPlayer = null;
    List<Player> myPlayerList = new List<Player>();     // 0:       me
                                                        // 1 - 4:   other players

    // key info
    const int maxKey = 4;
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
    public GameObject key1,key2,key3,key4,key5,key6,key7,key8,key9,key10;
    int Ran_Lab1,Ran_Lab2,Ran_Lab3,Ran_Lab4,Ran_Lab5;
    int Lab1Extra, Lab2Extra, Lab3Extra,Lab4Extra,Lab5Extra;

    //用作取得玩家數量的，決定鑰匙生成與否
    public int MaxGamePlayer;

    void Start()
    {
        _pv = this.gameObject.GetComponent<PhotonView>();
        print("LocalPlayer: " + PhotonNetwork.LocalPlayer);
        if(PhotonNetwork.IsConnected == false){
            SceneManager.LoadScene("StartScene");
        }
        else if(PhotonNetwork.CurrentRoom == null){
            SceneManager.LoadScene("LobbyScene");
        }
        if(PhotonNetwork.IsMasterClient){
            PickBoss();
        }
    }

    public List<int> generateRandom (int Length)
        {
            int Rand;
            List<int> list = new List<int>();
            list = new List<int>();
            for(int i = 0; i < Length; ++i){
                list.Add(-1);
            }
    
            for (int j = 0; j < Length; j++)
            {
                Rand = Random.Range(0,5);
    
                while (list.Contains(Rand))
                {
                    Rand = Random.Range(0,5);
                }
    
                list[j] = Rand;
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
            key6.SetActive(true);
            key6.transform.localPosition = positions1[SecondRandom[SecondRandomNum]];
        }
        key1.transform.localPosition = positions1[Ran_Lab1];
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
            key7.SetActive(true);
            key7.transform.localPosition = positions2[SecondRandom[SecondRandomNum]];
        }
        key2.transform.localPosition = positions2[Ran_Lab2];
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
            key8.SetActive(true);
            key8.transform.localPosition = positions3[SecondRandom[SecondRandomNum]];
        }
        key3.transform.localPosition = positions3[Ran_Lab3];
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
            key9.SetActive(true);
            key9.transform.localPosition = positions4[SecondRandom[SecondRandomNum]];
        }
        key4.transform.localPosition = positions4[Ran_Lab4];
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
            key10.SetActive(true);
            key10.transform.localPosition = positions5[SecondRandom[SecondRandomNum]];
        }
        key5.transform.localPosition = positions5[Ran_Lab5];
    }

    void PickBoss(){
        bossPlayer = PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount)];
        print("Boss is " + bossPlayer);
        _pv.RPC("RPC_Init", RpcTarget.All, bossPlayer);
    }

    [PunRPC]
    void RPC_Init(Player _bossPlayer){
        print("Initializing game...");
        // PhotonNetwork.LocalPlayer.NickName = "Test";
        bossPlayer = _bossPlayer;
        if(isBoss(PhotonNetwork.LocalPlayer)){
            PhotonNetwork.Instantiate("Boss_0", new Vector3(3f, 0.5f, -8f), Quaternion.identity);
            Instantiate(_freeLookBoss, new Vector3(2.47f, 0f, 1.501f), Quaternion.identity);
            RandomGenerateKey();
        }
        else{
            PhotonNetwork.Instantiate("Clone_0", new Vector3(-3f, 0.5f, -8f), Quaternion.identity);
            Instantiate(_freeLookClone, new Vector3(2.47f, 0f, 1.501f), Quaternion.identity);
        }
        InitUI();
        StartCoroutine(CountDown());
        StartCoroutine(ShowIdentity());
        print("Game initialized.");
    }

    void InitUI(){
        InitMyPlayerList();
        for(int i = 0; i < maxPlayer; ++i){
            _alivePlayerUI.transform.GetChild(i).gameObject.SetActive(true);
            _alivePlayerUI.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = myPlayerList[i].UserId;
            print("player " + i + ": " + myPlayerList[i].UserId);
            _deadPlayerUI.transform.GetChild(i).gameObject.SetActive(false);
            _deadPlayerUI.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = myPlayerList[i].UserId;
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
        myPlayerList.Add(PhotonNetwork.LocalPlayer);
        print("myPlayerList[0] = " + myPlayerList[0]);
        for(int i = 1; i < maxPlayer; ++i){
            myPlayerList.Add(PhotonNetwork.PlayerListOthers[i-1]);
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
        if(isBoss(PhotonNetwork.LocalPlayer)){
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

    public bool isBoss(Player player){
        return player == bossPlayer;
    }

    public void BossWin(){
        print("Boss win.");
        EndGame();
    }

    public void CloneWin(){
        print("Clone win.");
        EndGame();
    }

    public void Tie(){
        print("Tie.");
        EndGame();
    }

    void EndGame(){
        print("Ending game...");
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
            _deadPanel.SetActive(true);
        }
        _pv.RPC("RPC_SyncPlayer", RpcTarget.All, deadPlayer);
    }

    [PunRPC]
    void RPC_SyncPlayer(Player deadPlayer){
        int deadIndex = myPlayerList.FindIndex(x => x == deadPlayer);
        print(PhotonNetwork.LocalPlayer + ": deadIndex = " + deadIndex);
        _alivePlayerUI.transform.GetChild(deadIndex).gameObject.SetActive(false);
        _deadPlayerUI.transform.GetChild(deadIndex).gameObject.SetActive(true);
        _debuffPanel.SetActive(false);
        if(isAllDead()){
            BossWin();
        }
        else if(isBoss(deadPlayer)){
            CloneWin();
        }
    }

    bool isAllDead(){
        bool flag = false;
        for(int p = 1; p < maxPlayer; ++p){
            flag |= _deadPlayerUI.transform.GetChild(p).gameObject.activeSelf;
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

    // miniMap
    public int InRoom(float x, float z){
        resetMiniMap();
        int i;
        for(i = 0; i < MINIMAP_POSI_X.Length-2; ++i){
            if(i >= 13  && ((   MINIMAP_POSI_X[i][0] < x && x < MINIMAP_POSI_X[i][1]
                            &&  MINIMAP_POSI_Z[i][0] < z && z < MINIMAP_POSI_Z[i][1])
                        ||   (  MINIMAP_POSI_X[i+2][0] < x && x < MINIMAP_POSI_X[i+2][1]
                            &&  MINIMAP_POSI_Z[i+2][0] < z && z < MINIMAP_POSI_Z[i+2][1]))){
                    _miniMap.transform.GetChild(i+1).gameObject.SetActive(true);
                }
            else if(    MINIMAP_POSI_X[i][0] < x && x < MINIMAP_POSI_X[i][1]
                    &&  MINIMAP_POSI_Z[i][0] < z && z < MINIMAP_POSI_Z[i][1]){
                _miniMap.transform.GetChild(i+1).gameObject.SetActive(true);
            }
        }
        return i;
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
