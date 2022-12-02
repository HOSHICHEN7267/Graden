using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator Anime;
    private Rigidbody rigidbody;

    public float speed = 0.8f;
    public float gcTime = 2f;
    float GCtime = 0f;
    public float dieTime = 4f;
    float Dtime = 0f;

    bool isWalking = false;
    bool isDying = false;
    bool isGravityChange = false;
    bool useGravity = true;

    float Xrotate = 0f;
    float Zrotate = 0f;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!useGravity){
            rigidbody.AddForce(-1.0f * Physics.gravity * rigidbody.mass); // Add a force per frame to simulate the upside-down gravity
        }
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
            rigidbody.useGravity = false; // Turn off gravity, use force to simulate it (in Update)
            useGravity = false;
            Xrotate = 180f;
            transform.Translate(new Vector3(0f, 3.7f, 0f));
            transform.Rotate(0, 0, 180);
        }
        else
        {
            rigidbody.useGravity = true; // Turn on gravity
            useGravity = true;
            Xrotate = 0f;
            transform.Translate(new Vector3(0f, 3.7f, 0f));
            transform.Rotate(0, 0, -180);
        }
    }
}
