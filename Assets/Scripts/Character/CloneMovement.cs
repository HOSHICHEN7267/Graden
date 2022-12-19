using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CloneMovement : MonoBehaviour
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
    public float dieTime = 3f;
    public float putTime = 0.8f;
    float GCtime = 0f;
    float Dtime = 0f;
    float Ptime = 0f;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // controller variables
    bool isWalking = false;
    bool isDying = false;
    bool isPutting = false;
    bool isGravityChange = false;
    public bool useGravity = true;

    // key
    // public Text hasKeyText;
    // public Text totalKeyText;
    bool hasKey = false;
    int keyCount = 0;

    float Xrotate = 0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _gm = GameObject.FindObjectOfType<GameManager>();
        _puim = GameObject.FindObjectOfType<PlayerUIManager>();
        _pv = this.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_pv.IsMine){
            Control();
        }

        // display key text
        // if(hasKey){
        //     hasKeyText.text = "HasKey";
        // }
        // else{
        //     hasKeyText.text = "NoKey";
        // }
        // totalKeyText.text = keyCount + "/5";
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

        if (!useGravity)
        {
            rb.AddForce(-1.0f * Physics.gravity * GetComponent<Rigidbody>().mass); // Add a force per frame to simulate the upside-down gravity
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

        if (isDying)
        {
            Dtime += Time.deltaTime;
            if (Dtime > dieTime)
            {
                this.gameObject.SetActive(false);
            }
        }
        else if (isPutting)
        {
            Ptime += Time.deltaTime;
            if (_pv.IsMine && Ptime > putTime)
            {
                _puim.GiveKey();
                isPutting = false;
                Ptime = 0;
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
            if (playerPositionX < 16 && playerPositionX > -16 && playerPositionZ > -16 && playerPositionZ < 16)
            {
                _puim.EnterCenterLab();
            }
            else if (playerPositionX < -48 && playerPositionX > -92 && playerPositionZ > -20 && playerPositionZ < 20)
            {
                _puim.EnterLab1();
            }
            else if (playerPositionX < -28 && playerPositionX > -88 && playerPositionZ > 48 && playerPositionZ < 72)
            {
                _puim.EnterLab2();
            }
            else if (playerPositionX < 88 && playerPositionX > 28 && playerPositionZ > 48 && playerPositionZ < 72)
            {
                _puim.EnterLab3();
            }
            else if (playerPositionX < 92 && playerPositionX > 48 && playerPositionZ > -20 && playerPositionZ < 20)
            {
                _puim.EnterLab4();
            }
            else if (playerPositionX < 18.68 && playerPositionX > -18.92 && playerPositionZ > -90.46 && playerPositionZ < -48.51)
            {
                _puim.EnterLab5();
            }
            else if (playerPositionX < 28 && playerPositionX > -28 && playerPositionZ < 70 && playerPositionZ > 48){
                // Top
                _puim.EnterTop();
            }
            else if (playerPositionX < -63 && playerPositionX > -77 && playerPositionZ < 48 && playerPositionZ > 20){
                // Left
                _puim.EnterLeft();
            }
            else if (playerPositionX <  77&& playerPositionX >  63&& playerPositionZ < 48 && playerPositionZ > 20){
                // Right
                _puim.EnterRight();
            }
            else if (playerPositionX < 15 && playerPositionX > -15 && playerPositionZ < 48 && playerPositionZ > 14){
                // CenterTop
                _puim.EnterCenterTop();
            }
            else if (playerPositionX < 15 && playerPositionX > -15 && playerPositionZ < 14 && playerPositionZ > -48){
                // CenterBottom
                _puim.EnterCenterBottom();
            }
            else if (playerPositionX < -15 && playerPositionX > -48 && playerPositionZ < 48 && playerPositionZ > -48){
                // CenterLeft
                _puim.EnterCenterLeft();
            }
            else if (playerPositionX < 48 && playerPositionX > 15 && playerPositionZ < 48 && playerPositionZ > -48){
                // CenterRight
                _puim.EnterCenterRight();
            }
            else if (playerPositionX < -20 && playerPositionX > -93 && playerPositionZ < -65 && playerPositionZ > -77){
                // BottomLeft
                _puim.EnterBottomLeft();
            }
            else if (playerPositionX < -71 && playerPositionX > -93&& playerPositionZ < -20 && playerPositionZ > -65){
                // BottomLeft
                _puim.EnterBottomLeft();
            }
            else if (playerPositionX < 91 && playerPositionX > 20 && playerPositionZ < -57 && playerPositionZ > -66){
                // BottomRight
                _puim.EnterBottomRight();
            }
            else if (playerPositionX < 91 && playerPositionX > 80 && playerPositionZ < -20 && playerPositionZ > -57){
                // BottomRight
                _puim.EnterBottomRight();
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
        // Start here 12/9 1224
        if (_pv.IsMine && other.gameObject.tag == "Key" && !hasKey)
        {
            Destroy(other.gameObject);
            hasKey = true;
            _puim.GetKey();
        }
        if (_pv.IsMine && other.gameObject.tag == "Gravity" && !isGravityChange)
        {
            GravityChange();
            Debug.Log("GravityChanged");
        }
        if (other.gameObject.tag == "KeyCenter" && hasKey)
        {
            isPutting = true;
            Debug.Log("give key");
            keyCount++;
            hasKey = false;
        }
        if (other.gameObject.tag == "Boss")
        {
            isDying = true;
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
        if (isDying)
        {
            Anime.SetInteger("Status", 4);
        }
        else if (isPutting)
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
