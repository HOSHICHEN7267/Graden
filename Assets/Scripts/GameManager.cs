using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PhotonView _pv;

    // player info
    int bossPlayer = -1;
    public GameObject[] _identityUI;    // 0:   boss
                                        // 1:   clone

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

    // // mini map
    // GameObject center;
    // Sprite miniMap,miniMap_center,miniMap_Lab1,miniMap_Lab2,miniMap_Lab3,miniMap_Lab4,miniMap_Lab5;

    // GameObject playerPosition = null;

    void Start()
    {
        _pv = GetComponent<PhotonView>();
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

    void Update()
    {
        // SwitchMap();
    }

    // public void SwitchMap(){
    //     center = GameObject.Find("Canvas/AspectRatioFitter/MiniMaps/miniMap");
    //     miniMap = Resources.Load<Sprite>("Sprite/miniMap");
    //     miniMap_center = Resources.Load<Sprite>("Sprite/miniMap_center");
    //     miniMap_Lab1 = Resources.Load<Sprite>("Sprite/miniMap_Lab1");
    //     miniMap_Lab2 = Resources.Load<Sprite>("Sprite/miniMap_Lab2");
    //     miniMap_Lab3 = Resources.Load<Sprite>("Sprite/miniMap_Lab3");
    //     miniMap_Lab4 = Resources.Load<Sprite>("Sprite/miniMap_Lab4");
    //     miniMap_Lab5 = Resources.Load<Sprite>("Sprite/miniMap_Lab5");
    //     playerPosition = GameObject.FindGameObjectWithTag("Player");
    //     float playerPositionX = playerPosition.transform.position.x;
    //     float playerPositionZ = playerPosition.transform.position.z;

    //     if (playerPositionX < 16 && playerPositionX > -16 && playerPositionZ > -16 && playerPositionZ < 16){
    //         center.GetComponent<Image>().sprite = miniMap_center;
    //     }
    //     else if (playerPositionX < -48 && playerPositionX > -92 && playerPositionZ > -20 && playerPositionZ < 20){
    //         center.GetComponent<Image>().sprite = miniMap_Lab1;
    //     }
    //     else if (playerPositionX < -28 && playerPositionX > -88 && playerPositionZ > 48 && playerPositionZ < 72){
    //         center.GetComponent<Image>().sprite = miniMap_Lab2;
    //     }
    //     else if (playerPositionX < 110 && playerPositionX > 48 && playerPositionZ > 48 && playerPositionZ < 72){
    //         center.GetComponent<Image>().sprite = miniMap_Lab3;
    //     }
    //     else if (playerPositionX < 118 && playerPositionX > 70 && playerPositionZ > -20 && playerPositionZ < 20){
    //         center.GetComponent<Image>().sprite = miniMap_Lab4;
    //     }
    //     else if (playerPositionX < 18.68 && playerPositionX > -18.92 && playerPositionZ > -90.46 && playerPositionZ < -48.51){
    //         center.GetComponent<Image>().sprite = miniMap_Lab5;
    //     }
    //     else{
    //         center.GetComponent<Image>().sprite = miniMap;
    //     }
        
    // }

    public List<int> generateRandom (int Length)
        {
            int Rand;
            List<int> list = new List<int>();
            list = new List<int>(new int[Length]);
    
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
                SecondRandom[i] = i;
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
                SecondRandom[i] = i;
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
                SecondRandom[i] = i;
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
                SecondRandom[i] = i;
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
                SecondRandom[i] = i;
            }
            SecondRandom.Remove(Ran_Lab5);
            int SecondRandomNum = Random.Range(0,4);
            key10.SetActive(true);
            key10.transform.localPosition = positions5[SecondRandom[SecondRandomNum]];
        }
        key5.transform.localPosition = positions5[Ran_Lab5];
    }

    void PickBoss(){
        bossPlayer = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        print("Boss is " + bossPlayer);
        _pv.RPC("RPC_Init", RpcTarget.All, bossPlayer);
    }

    [PunRPC]
    void RPC_Init(int num){
        bossPlayer = num;
        print("Initializing game...");
        while(bossPlayer == -1){}
        if(isBoss()){
            PhotonNetwork.Instantiate("Boss_0", new Vector3(3f, 0.5f, -8f), Quaternion.identity);
            Instantiate(_freeLookBoss, new Vector3(2.47f, 0f, 1.501f), Quaternion.identity);
        }
        else{
            PhotonNetwork.Instantiate("Clone_0", new Vector3(-3f, 0.5f, -8f), Quaternion.identity);
            Instantiate(_freeLookClone, new Vector3(2.47f, 0f, 1.501f), Quaternion.identity);
        }
        RandomGenerateKey();
        // SwitchMap();
        StartCoroutine(CountDown());
        StartCoroutine(ShowIdentity());
        print("Game initialized.");
    }

    IEnumerator ShowIdentity(){
        if(isBoss()){
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

    bool isBoss(){
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[bossPlayer];
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
}
