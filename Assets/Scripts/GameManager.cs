using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

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
    public List<GameObject> _keyUI; // 0:   no key
                                    // 1:   has key
    public Text _totalKeyText;
    int totalKey;

    // gravity info
    public List<GameObject> _gravityUI; // 0:   not change
                                        // 1:   changed
    
    // debuff panel
    public GameObject _debuffPanel;

    // timer
    public Text _timerText;
    public int timer_totalSec;
    int timer_min = 0;
    int timer_sec = 0;

    // camera
    public GameObject _freeLookBoss;
    public GameObject _freeLookClone;

    public GameObject key1,key2,key3,key4,key5;
    int Ran_Lab1,Ran_Lab2,Ran_Lab3,Ran_Lab4,Ran_Lab5;

    GameObject center;
    Sprite miniMap,miniMap_center,miniMap_Lab1,miniMap_Lab2,miniMap_Lab3,miniMap_Lab4,miniMap_Lab5;

    GameObject playerPosition = null;
    void Start()
    {
        print("LocalPlayer: " + PhotonNetwork.LocalPlayer);
        if(PhotonNetwork.IsConnected == false){
            SceneManager.LoadScene("StartScene");
        }
        else if(PhotonNetwork.CurrentRoom == null){
            SceneManager.LoadScene("LobbyScene");
        }
        else{
            StartCoroutine( InitGame() );
        }
        // if(PhotonNetwork.IsConnected == false){
        //     SceneManager.LoadScene("StartScene");
        // }
        // else{
        //     InitUI();
        //     StartCoroutine(CountDown());
        // }
    }

    void Update()
    {
        SwitchMap();
    }

    public void SwitchMap(){
        center = GameObject.Find("Canvas/AspectRatioFitter/MiniMaps/miniMap");
        miniMap = Resources.Load<Sprite>("Sprite/miniMap");
        miniMap_center = Resources.Load<Sprite>("Sprite/miniMap_center");
        miniMap_Lab1 = Resources.Load<Sprite>("Sprite/miniMap_Lab1");
        miniMap_Lab2 = Resources.Load<Sprite>("Sprite/miniMap_Lab2");
        miniMap_Lab3 = Resources.Load<Sprite>("Sprite/miniMap_Lab3");
        miniMap_Lab4 = Resources.Load<Sprite>("Sprite/miniMap_Lab4");
        miniMap_Lab5 = Resources.Load<Sprite>("Sprite/miniMap_Lab5");
        playerPosition = GameObject.FindGameObjectWithTag("Player");
        float playerPositionX = playerPosition.transform.position.x;
        float playerPositionZ = playerPosition.transform.position.z;

        if (playerPositionX < 16 && playerPositionX > -16 && playerPositionZ > -16 && playerPositionZ < 16){
            center.GetComponent<Image>().sprite = miniMap_center;
        }
        else if (playerPositionX < -48 && playerPositionX > -92 && playerPositionZ > -20 && playerPositionZ < 20){
            center.GetComponent<Image>().sprite = miniMap_Lab1;
        }
        else if (playerPositionX < -28 && playerPositionX > -88 && playerPositionZ > 48 && playerPositionZ < 72){
            center.GetComponent<Image>().sprite = miniMap_Lab2;
        }
        else if (playerPositionX < 110 && playerPositionX > 48 && playerPositionZ > 48 && playerPositionZ < 72){
            center.GetComponent<Image>().sprite = miniMap_Lab3;
        }
        else if (playerPositionX < 118 && playerPositionX > 70 && playerPositionZ > -20 && playerPositionZ < 20){
            center.GetComponent<Image>().sprite = miniMap_Lab4;
        }
        else if (playerPositionX < 18.68 && playerPositionX > -18.92 && playerPositionZ > -90.46 && playerPositionZ < -48.51){
            center.GetComponent<Image>().sprite = miniMap_Lab5;
        }
        else{
            center.GetComponent<Image>().sprite = miniMap;
        }
        
    }

    public void RandomGenerateKey(){
        // Lab 1
        Vector3[] positions1 = new Vector3[5];
        positions1[0] = new Vector3(-84.225f,0.88f,7.35f);
        positions1[1] = new Vector3(-64.97f,0.88f,-10.23f);
        positions1[2] = new Vector3(-64.77f,7.32f,-5.7f);
        positions1[3] = new Vector3(-64.8f,4.83f,-11.43f);
        positions1[4] = new Vector3(-75.41f,0.72f,-17.73f);
        Ran_Lab1 = Random.Range(0,5);
        key1.transform.localPosition = positions1[Ran_Lab1];
        // Lab 2
        Vector3[] positions2 = new Vector3[5];
        positions2[0] = new Vector3(-45.802f,4.83f,49.81f);
        positions2[1] = new Vector3(-47.01f,4.83f,41.07f);
        positions2[2] = new Vector3(-48.08f,-2.151f,42.33f);
        positions2[3] = new Vector3(-48.08f,0.61f,52.66f);
        positions2[4] = new Vector3(-38.412f,-1.835f,40.73f);
        Ran_Lab2 = Random.Range(0,5);
        key2.transform.localPosition = positions2[Ran_Lab2];
        // Lab 3
        Vector3[] positions3 = new Vector3[5];
        positions3[0] = new Vector3(67.94f,1.76f,49f);
        positions3[1] = new Vector3(80.74f,-2.151f,41.24f);
        positions3[2] = new Vector3(78.779f,1.89f,49.101f);
        positions3[3] = new Vector3(88.946f,1.692f,50.31f);
        positions3[4] = new Vector3(104.826f,1.872f,50.848f);
        Ran_Lab3 = Random.Range(0,5);
        key3.transform.localPosition = positions3[Ran_Lab3];
        // Lab 4
        Vector3[] positions4 = new Vector3[5];
        positions4[0] = new Vector3(93.14f,1.84f,-11.46f);
        positions4[1] = new Vector3(95.69f,-2.151f,1.16f);
        positions4[2] = new Vector3(108.103f,-2.13f,42.33f);
        positions4[3] = new Vector3(94.101f,1.946f,6.117f);
        positions4[4] = new Vector3(72.88f,4.44f,-10.69f);
        Ran_Lab4 = Random.Range(0,5);
        key4.transform.localPosition = positions4[Ran_Lab4];
        // Lab 5
        Vector3[] positions5 = new Vector3[5];
        positions5[0] = new Vector3(3f,3.3f,-71.6f);
        positions5[1] = new Vector3(17.45f,4.83f,-76.78f);
        positions5[2] = new Vector3(11.05f,4.83f,-57.4f);
        positions5[3] = new Vector3(-11.07f,0.88f,-58.58f);
        positions5[4] = new Vector3(-14.7f,0.88f,-89.87f);
        Ran_Lab5 = Random.Range(0,5);
        key5.transform.localPosition = positions5[Ran_Lab5];
    }

    public void GetKey(){
        ++totalKey;
        _keyUI[0].SetActive(false);
        _keyUI[1].SetActive(true);
        _totalKeyText.text = totalKey.ToString() + "  /  " + maxKey.ToString();
    }

    public void PutKey(){
        _keyUI[0].SetActive(true);
        _keyUI[1].SetActive(false);
    }

    public void PlayerDie(Player deadPlayer){
        int deadIndex = myPlayerList.FindIndex(x => x == deadPlayer);
        _alivePlayerUI[deadIndex].SetActive(false);
        _deadPlayerUI[deadIndex].SetActive(true);
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

    void InitUI(){
        InitMyPlayerList();
        for(int i=0; i < maxPlayer; ++i){
            _alivePlayerUI[i].SetActive(true);
            _deadPlayerUI[i].SetActive(false);
        }
        _keyUI[0].SetActive(true);
        _keyUI[1].SetActive(false);
        totalKey = 0;
        _totalKeyText.text = totalKey.ToString() + "  /  " + maxKey.ToString();
        timer_min = timer_totalSec / 60;
        timer_sec = timer_totalSec % 60;
    }

    void InitMyPlayerList(){
        // myPlayerList[0] = PhotonNetwork.LocalPlayer;
        // for(int i=1; i < maxPlayer; ++i){
        //     myPlayerList[i] = PhotonNetwork.PlayerListOthers[i-1];
        // }
    }

    IEnumerator InitGame(){
        yield return new WaitForSeconds(1);
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Instantiate("Boss", new Vector3(3f, 0f, -8f), Quaternion.identity);
            Instantiate(_freeLookBoss, new Vector3(2.47f, 0f, 1.501f), Quaternion.identity);
        }
        else{
            PhotonNetwork.Instantiate("Clone", new Vector3(-3f, 0f, -8f), Quaternion.identity);
            Instantiate(_freeLookClone, new Vector3(2.47f, 0f, 1.501f), Quaternion.identity);
        }
        InitUI();
        RandomGenerateKey();
        SwitchMap();
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown(){
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
}
