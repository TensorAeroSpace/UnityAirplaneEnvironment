using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birds : MonoBehaviour
{
    public Rigidbody aircraftRigidBody;
    public float previousTime, interval;

    void FixedUpdate()
    {
        if(Time.time > previousTime + interval)
        {
            aircraftRigidBody.AddForce(new Vector3(0, -1000, 0));
            previousTime = Time.time;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        previousTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
