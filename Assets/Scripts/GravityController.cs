using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField]float Xrotate = 180f;
    [SerializeField]float Zrotate;

    [SerializeField]float moveSpeed = 2.5f;

    bool haveKey = false;
    bool closeKey = false;
    bool closeGra = false;

    [SerializeField] GameObject key;
    [SerializeField] GameObject gravity;

    public GameController gameController;
    public UpsideDown upsideDown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xvalue = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float zvalue = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        transform.Translate(xvalue, 0, zvalue);
        if (Input.GetKeyDown("space")){
            transform.Rotate(Xrotate, 0 ,Zrotate);
        }

        if(Input.GetKeyDown(KeyCode.F)){
            gameController.CloseMessagePanel();
            if ((closeKey == true && key.tag == "Key") && haveKey == false){
                Debug.Log("Get Key");
                haveKey = true;
                Destroy(key);
            }
            if (closeGra == true && gravity.tag == "Gravity"){
                Debug.Log("Gravity Activate");
                transform.Rotate(Xrotate, 0 ,Zrotate);
            }
        }

    }

    private void OnTriggerEnter(Collider other) {
        gameController.OpenMessagePanel();
        if (other.gameObject.tag == "Key"){
            closeKey = true;
        }
        if (other.gameObject.tag == "Gravity"){
            closeGra = true;
        }
        
    }

    private void OnTriggerExit(Collider other) {
        gameController.CloseMessagePanel();
        if (other.gameObject.tag == "Key"){
            closeKey = false;
        }
        if (other.gameObject.tag == "Gravity"){
            closeGra = false;
        }
    }
}
