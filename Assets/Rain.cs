using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    public Rigidbody aircraftRigidBody;
    public float impact;

    void FixedUpdate()
    {
        aircraftRigidBody.AddForce(new Vector3(0, -1, 0) * impact);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
