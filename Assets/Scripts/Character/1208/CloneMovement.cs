using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneMovement : MonoBehaviour
{
    public Animator Anime;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public Transform orientation;

    [Header("Time")]
    public float gcTime = 2f;
    public float dieTime = 4f;
    float GCtime = 0f;
    float Dtime = 0f;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // controller variables
    bool isWalking = false;
    bool isDying = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();

        // handle drag
        rb.drag = groundDrag;

        // display key text
        // if(hasKey){
        //     hasKeyText.text = "HasKey";
        // }
        // else{
        //     hasKeyText.text = "NoKey";
        // }
        // totalKeyText.text = keyCount + "/5";
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
                isDying = false;
                Dtime = 0;
            }
        }
        else
        {
            MovePlayer();
            SetAnime();
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
            Destroy(other.gameObject);
            hasKey = true;
        }
        if (other.gameObject.tag == "Gravity" && !isGravityChange)
        {
            GravityChange();
            Debug.Log("GravityChanged");
        }
        if(other.gameObject.tag == "KeyCenter" && hasKey){
            Debug.Log("give key");
            keyCount++;
            hasKey = false;
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
        }
        else
        {
            rb.useGravity = true; // Turn on gravity
            useGravity = true;
            Xrotate = 0f;
            transform.Translate(new Vector3(0f, 3.7f, 0f));
            transform.Rotate(0, 0, -180);
        }
    }

    private void SetAnime()
    {
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
