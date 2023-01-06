using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuSceneManager : MonoBehaviourPunCallbacks
{
    public GameObject _checkPanel;

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
        StartCoroutine(FadeInCheckPanel());
    }

    public void OnClickQuitGameNo(){
        print("[Click Quit Game (No)]");
        FadeOutCheckPanel();
    }

    public void OnClickQuitGameYes(){
        print("[Click Quit Game (Yes)]");
        Application.Quit();
    }

    IEnumerator FadeInCheckPanel(){
        _checkPanel.SetActive(true);
        // panel
        float originR = _checkPanel.GetComponent<Image>().color.r;
        float originB = _checkPanel.GetComponent<Image>().color.b;
        float originG = _checkPanel.GetComponent<Image>().color.g;
        float originAlpha = _checkPanel.GetComponent<Image>().color.a;
        float alpha = 0;
        float unit = originAlpha/5f;
        _checkPanel.GetComponent<Image>().color = new Color(originR, originB, originG, alpha);
        while(alpha < originAlpha){
            alpha += unit;
            _checkPanel.GetComponent<Image>().color = new Color(originR, originB, originG, alpha);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        // window
        GameObject _checkWindow = _checkPanel.transform.GetChild(0).gameObject;
        _checkWindow.SetActive(true);
        float originX = _checkWindow.transform.localScale.x;
        float originY = _checkWindow.transform.localScale.y;
        float originZ = _checkWindow.transform.localScale.z;
        float y = 0;
        unit = originY/5f;
        _checkWindow.transform.localScale = new Vector3(originX, originY, originZ);
        while(y < originY){
            y += unit;
            _checkWindow.transform.localScale = new Vector3(originX, y, originZ);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    void FadeOutCheckPanel(){
        _checkPanel.transform.GetChild(0).gameObject.SetActive(false);
        _checkPanel.SetActive(false);
    }
    
    public override void OnConnectedToMaster(){
        print("Connected! Joining Lobby...");
        SceneManager.LoadScene("LobbyScene");
    }
}
