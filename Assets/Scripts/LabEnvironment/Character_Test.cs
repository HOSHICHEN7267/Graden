using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Test : MonoBehaviour
{
    public float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dx = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        float dz = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
        this.transform.Translate(new Vector3(dx, 0, 0));
        this.transform.Translate(new Vector3(0, 0, dz));
    }
}
