using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CloneMovement : MonoBehaviourPunCallbacks
{
    GameManager _gm;
    PhotonView _pv;

    public GameObject particle;

    public Animator Anime;

    private AudioSource audioSource;

    public AudioClip fx_getKey;
    public AudioClip fx_putKey;
    public AudioClip fx_gravityPlate;

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
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _gm = GameObject.FindObjectOfType<GameManager>();
        _pv = this.gameObject.GetComponent<PhotonView>();
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
                particle.SetActive(false);
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
                print(PhotonNetwork.LocalPlayer + " died");
                if(_pv.IsMine){
                    _gm.PlayerDie(PhotonNetwork.LocalPlayer);
                }
                this.gameObject.SetActive(false);
                // PhotonNetwork.Destroy(this.gameObject);
            }
        }
        else if (isPutting)
        {
            Ptime += Time.deltaTime;
            if (_pv.IsMine && Ptime > putTime)
            {
                _gm.GiveKey();
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
            _gm.InRoom(playerPositionX, playerPositionZ);
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
        if (other.gameObject.tag == "Key" && !hasKey)
        {
            audioSource.PlayOneShot(fx_getKey);
            Destroy(other.gameObject);
            if(_pv.IsMine){
                hasKey = true;
                _gm.GetKey();
            }
        }
        if (_pv.IsMine && other.gameObject.tag == "Gravity" && !isGravityChange)
        {
            audioSource.PlayOneShot(fx_gravityPlate);
            particle.SetActive(true);
            GravityChange();
        }
        if (other.gameObject.tag == "KeyCenter" && hasKey)
        {
            audioSource.PlayOneShot(fx_putKey);
            isPutting = true;
            Debug.Log("give key");
            keyCount++;
            hasKey = false;
        }
        if (_pv.IsMine && other.gameObject.tag == "Boss")
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
            _gm.GravityChange();
        }
        else
        {
            rb.useGravity = true; // Turn on gravity
            useGravity = true;
            Xrotate = 0f;
            transform.Translate(new Vector3(0f, 3.7f, 0f));
            transform.Rotate(0, 0, -180);
            _gm.GravityChange();
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

    public override void OnLeftRoom(){
        if(this.gameObject.activeSelf){
            this.gameObject.SetActive(false);
        }
    }
}
