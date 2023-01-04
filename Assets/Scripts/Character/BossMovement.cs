using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossMovement : MonoBehaviourPunCallbacks
{
    GameManager _gm;
    PhotonView _pv;
    
    public Animator Anime;

    private AudioSource audioSource;
    public AudioClip fx_gravityJump;
    public AudioClip fx_kill;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public Transform orientation;

    [Header("Time")]
    public float gcTime = 2f;
    public float killTime = 3f;
    public float slowTime = 5f;
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
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _gm = GameObject.FindObjectOfType<GameManager>();
        _pv = this.gameObject.GetComponent<PhotonView>();
        if(_pv.IsMine){
            _gm.ChangeToBossKey();
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
            audioSource.PlayOneShot(fx_gravityJump);
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
            inCenter = (_gm.InRoom(playerPositionX, playerPositionZ) == 0);
        }

        // slow
        if (inCenter)
        {
            if (Stime < slowTime)
            {
                Stime += Time.deltaTime;
            }
            else if (!isSlow)
            {
                print("is slow now");
                isSlow = true;
                moveSpeed = moveSpeed / 2;
                if(_pv.IsMine){
                    _gm.SlowSpeed();
                }
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
                _gm.NormalSpeed();
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
        if (other.gameObject.tag == "Clone")
        {
            audioSource.PlayOneShot(fx_kill);
            isKilling = true;
            StartCoroutine(CleanUpDeadBody(other.gameObject));
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

    public override void OnLeftRoom(){
        _gm.CloneWin();
        this.gameObject.SetActive(false);
    }

    IEnumerator CleanUpDeadBody(GameObject deadPlayer){
        float dieTime = 1f + deadPlayer.gameObject.GetComponent<CloneMovement>().dieTime;
        yield return new WaitForSeconds(dieTime);
        deadPlayer.SetActive(false);
    }
}
