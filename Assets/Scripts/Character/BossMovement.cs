using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossMovement : MonoBehaviour
{
    public Animator Anime;
    public GameObject GameManager;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public Transform orientation;

    [Header("Time")]
    public float gcTime = 2f;
    public float killTime = 3f;
    float GCtime = 0f;
    float Ktime = 0f;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    bool isWalking = false;
    bool isKilling = false;
    bool isGravityChange = false;
    public bool useGravity = true;

    float Xrotate = 0f;
    
    PhotonView _pv;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _pv = this.gameObject.GetComponent<PhotonView>();
        GameManager.GetComponent<GameManager>().HideKeyStatus();
    }

    // Update is called once per frame
    void Update()
    {
        if(_pv.IsMine){
            Control();
        }
    }

    void Control()
    {
        MyInput();
        SpeedControl();

        // handle drag
        rb.drag = groundDrag;
    }

    void FixedUpdate()
    {
        if (isKilling)
        {
            Debug.Log("killing");
        }

        if (!useGravity)
        {
            rb.AddForce(-1.0f * Physics.gravity * GetComponent<Rigidbody>().mass); // Add a force per frame to simulate the upside-down gravity
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

        if(horizontalInput != 0 || verticalInput != 0){
            isWalking = true;
        }
        else{
            isWalking = false;
        }

        if (isKilling)
        {
            Ktime += Time.deltaTime;
            if (Ktime > killTime)
            {
                isKilling = false;
                Ktime = 0;
            }
        }
        else
        {
            MovePlayer();
        }
        SetAnime();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal") * (useGravity ? 1 : -1);
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            isKilling = true;
            //Destroy(other.gameObject);
        }
    }

    private void GravityChange()
    {
        isGravityChange = true;
        if (Xrotate == 0f)
        {
            rb.useGravity = false; // Turn off gravity, use force to simulate it (in Update)
            useGravity = false;
            Xrotate = 180f;
            transform.Translate(new Vector3(0f, 3.7f, 0f));
            transform.Rotate(0, 0, 180);
            GameManager.GetComponent<GameManager>().GravityChange();
        }
        else
        {
            rb.useGravity = true; // Turn on gravity
            useGravity = true;
            Xrotate = 0f;
            transform.Translate(new Vector3(0f, 3.7f, 0f));
            transform.Rotate(0, 0, -180);
            GameManager.GetComponent<GameManager>().GravityChange();
        }
    }

    private void SetAnime()
    {

        if (isKilling)
        {
            Anime.SetInteger("Status", 2);
        }
        else if (isWalking)
        {
            Anime.SetInteger("Status", 1);
        } 
        else
        {
            Anime.SetInteger("Status", 0);
        }
    }
}
