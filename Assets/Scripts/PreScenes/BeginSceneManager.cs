using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginSceneManager : MonoBehaviour
{
    public void OnClickStart(){
        print("[Click Start]");
        SceneManager.LoadScene("MenuScene");
    }
}
