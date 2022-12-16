using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossMovement : MonoBehaviour
{
    GameManager _gm;
    PlayerUIManager _puim;
    PhotonView _pv;
    
    public Animator Anime;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public Transform orientation;

    [Header("Time")]
    public float gcTime = 2f;
    public float killTime = 3f;
    public float slowTime = 20f;
    float GCtime = 0f;
    float Ktime = 0f;
    float Stime = 0f;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    bool isWalking = false;
    bool isKilling = false;
    bool isGravityChange = false;
    bool inCenter = false;
    bool isSlow = false;
    public bool useGravity = true;

    float Xrotate = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _gm = GameObject.FindObjectOfType<GameManager>();
        _puim = GameObject.FindObjectOfType<PlayerUIManager>();
        _pv = this.gameObject.GetComponent<PhotonView>();
        if(_pv.IsMine){
            _puim.ChangeToBossKey();
        }
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

        if (_pv.IsMine && Input.GetKeyDown("space") && isGravityChange == false)
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

        // miniMap
        float playerPositionX = this.transform.position.x;
        float playerPositionZ = this.transform.position.z;
        if(_pv.IsMine){
            if (playerPositionX < 16 && playerPositionX > -16 && playerPositionZ > -16 && playerPositionZ < 16){
                _puim.EnterCenterLab();
                inCenter = true;
            }
            else if (playerPositionX < -48 && playerPositionX > -92 && playerPositionZ > -20 && playerPositionZ < 20){
                _puim.EnterLab1();
                inCenter = false;
            }
            else if (playerPositionX < -28 && playerPositionX > -88 && playerPositionZ > 48 && playerPositionZ < 72)
            {
                _puim.EnterLab2();
                inCenter = false;
            }
            else if (playerPositionX < 110 && playerPositionX > 48 && playerPositionZ > 48 && playerPositionZ < 72)
            {
                _puim.EnterLab3();
                inCenter = false;
            }
            else if (playerPositionX < 118 && playerPositionX > 70 && playerPositionZ > -20 && playerPositionZ < 20)
            {
                _puim.EnterLab4();
                inCenter = false;
            }
            else if (playerPositionX < 18.68 && playerPositionX > -18.92 && playerPositionZ > -90.46 && playerPositionZ < -48.51)
            {
                _puim.EnterLab5();
                inCenter = false;
            }
            else
            {
                _puim.InCorridor();
                inCenter = false;
            }
        }

        // slow
        if (inCenter)
        {
            if (Stime < slowTime)
            {
                Stime += Time.deltaTime;
            }
            else if (_pv.IsMine && !isSlow)
            {
                isSlow = true;
                moveSpeed = moveSpeed / 2;
                _puim.SlowSpeed();
                Debug.Log("slow...");
            }
        }
        else
        {
            Stime = 0;
            if (isSlow)
            {
                isSlow = false;
                moveSpeed = moveSpeed * 2;
                _puim.NormalSpeed();
                Debug.Log("no slow");
            }
        }
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
            _puim.GravityChange();
        }
        else
        {
            rb.useGravity = true; // Turn on gravity
            useGravity = true;
            Xrotate = 0f;
            transform.Translate(new Vector3(0f, 3.7f, 0f));
            transform.Rotate(0, 0, -180);
            _puim.GravityChange();
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
