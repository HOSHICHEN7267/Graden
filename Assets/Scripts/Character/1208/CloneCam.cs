using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CloneCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Clone");
        foreach(GameObject p in playerList){
            if(p.GetComponent<PhotonView>().IsMine){
                player = p.transform;
                orientation = player.GetChild(1);
                playerObj = player.GetChild(0).transform;
                rb = p.GetComponent<Rigidbody>();
                this.GetComponent<CinemachineFreeLook>().Follow = player;
                this.GetComponent<CinemachineFreeLook>().LookAt = player;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput * (player.GetComponent<CloneMovement>().useGravity ? 1 : -1);

        if(inputDir != Vector3.zero){
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        playerObj.localRotation = Quaternion.Euler(0, playerObj.localRotation.eulerAngles.y, 0);
    }
}
