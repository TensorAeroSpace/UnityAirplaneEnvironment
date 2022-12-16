using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCube : MonoBehaviour
{
    public PlaneAgent plane;

    public void ResetPos()
    {
        gameObject.transform.position = plane.gameObject.transform.position + UnityEngine.Random.onUnitSphere * 10;
        //gameObject.transform.position = plane.gameObject.transform.position + plane.gameObject.transform.forward * 10;
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetPos();
    }

    private void OnTriggerEnter(Collider other)
    {
        plane.Win();
        ResetPos();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
