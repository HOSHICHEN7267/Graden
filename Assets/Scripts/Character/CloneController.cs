using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    public Animator Anime;

    public float speed = 0.8f;
    public float dieTime = 4f;
    float time = 0f;

    bool isWalking = false;
    bool isDying = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDying)
        {
            time += Time.deltaTime;
            if (time > dieTime)
            {
                isDying = false;
                time = 0;
            }
        }
        else
        {
            isWalking = false;

            if (Input.GetKey(KeyCode.D))
            {
                isWalking = true;
                Quaternion newRotation = Quaternion.Euler(0, 90f, 0);
                this.gameObject.transform.rotation = newRotation;
                this.gameObject.transform.position += this.gameObject.transform.forward * speed * Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                isWalking = true;
                Quaternion newRotation = Quaternion.Euler(0, -90f, 0);
                this.gameObject.transform.rotation = newRotation;
                this.gameObject.transform.position += this.gameObject.transform.forward * speed * Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.W))
            {
                isWalking = true;
                Quaternion newRotation = Quaternion.Euler(0, 0, 0);
                this.gameObject.transform.rotation = newRotation;
                this.gameObject.transform.position += this.gameObject.transform.forward * speed * Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                isWalking = true;
                Quaternion newRotation = Quaternion.Euler(0, 180f, 0);
                this.gameObject.transform.rotation = newRotation;
                this.gameObject.transform.position += this.gameObject.transform.forward * speed * Time.fixedDeltaTime;
            }

            if (isWalking)
            {
                Anime.SetInteger("Status", 1);
            }
            else
            {
                Anime.SetInteger("Status", 0);
            }
        }
    }
}
