using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    public TargetCube target;
    public GameObject marker, marker2;
    public float range;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 new_vec = target.transform.position - transform.position;
        new_vec /= new_vec.magnitude;
        marker.transform.position = transform.position + new_vec * range;
        
        marker2.transform.position = transform.position + transform.forward * range;
    }
}
