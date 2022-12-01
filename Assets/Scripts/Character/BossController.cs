using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator Anime;

    public float speed = 0.8f;
    public float gcTime = 2f;
    float GCtime = 0f;
    public float dieTime = 4f;
    float Dtime = 0f;

    bool isWalking = false;
    bool isDying = false;
    bool isGravityChange = false;

    float Xrotate = 0f;
    float Zrotate = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("space") && isGravityChange == false)
        {
            GravityChange();
        }
        if (isGravityChange)
        {
            GCtime += Time.deltaTime;
            if (GCtime >= gcTime)
            {
                isGravityChange = false;
                GCtime = 0;
            }
        }
        if (isDying)
        {
            Dtime += Time.deltaTime;
            if (Dtime > dieTime)
            {
                isDying = false;
                Dtime = 0;
            }
        }
        else
        {
            isWalking = false;

            if (Input.GetKey(KeyCode.D))
            {
                isWalking = true;
                Quaternion newRotation = Quaternion.Euler(Xrotate, 90f, Zrotate);
                this.gameObject.transform.rotation = newRotation;
                this.gameObject.transform.position += this.gameObject.transform.forward * speed * Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                isWalking = true;
                Quaternion newRotation = Quaternion.Euler(Xrotate, -90f, Zrotate);
                this.gameObject.transform.rotation = newRotation;
                this.gameObject.transform.position += this.gameObject.transform.forward * speed * Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.W))
            {
                isWalking = true;
                Quaternion newRotation = Quaternion.Euler(Xrotate, 0, Zrotate);
                this.gameObject.transform.rotation = newRotation;
                this.gameObject.transform.position += this.gameObject.transform.forward * speed * Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                isWalking = true;
                Quaternion newRotation = Quaternion.Euler(Xrotate, 180f, Zrotate);
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
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
        }
    }
    void GravityChange()
    {
        isGravityChange = true;
        if (Xrotate == 0f)
        {
            Xrotate = 180f;
            transform.Translate(new Vector3(0f, 3.7f, 0f));
            Physics.gravity = new Vector3(0, 1f, 0);
            transform.Rotate(180, 0, 0);
        }
        else
        {
            Xrotate = 0f;
            transform.Translate(new Vector3(0f, 3.7f, 0f));
            Physics.gravity = new Vector3(0, -1f, 0);
            transform.Rotate(-180, 0, 0);
        }
    }
}
