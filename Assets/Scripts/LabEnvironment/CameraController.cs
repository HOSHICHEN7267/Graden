using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 initPos = new Vector3(0f, 2f, 0f);
    void Start()
    {
        this.gameObject.transform.position = initPos;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        float v = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
        float cos = Mathf.Cos( this.gameObject.transform.eulerAngles.y * Mathf.Deg2Rad );
        float sin = Mathf.Sin( this.gameObject.transform.eulerAngles.y * Mathf.Deg2Rad );
        float dx = h*cos + v*sin;
        float dz = h*sin + v*cos;
        this.transform.position += new Vector3(dx, 0f, dz);
        this.gameObject.transform.Rotate( new Vector3(0f, h * speed * speed, 0f) );
    }
}
