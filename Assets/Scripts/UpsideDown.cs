using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpsideDown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            transform.Rotate(0, 0*Time.deltaTime, 180);
        }
    }
}
