using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpeed : MonoBehaviour
{
    public float duTime = 30f;
    float timer = 0f;
    bool inCenter = true;
    bool isSlow = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (inCenter)
        {
            if (timer < duTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                isSlow = true;
                Debug.Log("slow...");
            }
        }
        else
        {
            isSlow = false;
            Debug.Log("no slow");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GameController")
        {
            inCenter = !inCenter;
        }
    }
}
