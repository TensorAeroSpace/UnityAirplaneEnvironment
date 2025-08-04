using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCube : MonoBehaviour
{
    public FlyingAgent plane;
    public Rigidbody plane2;

    public void ResetPos()
    {
        //Vector3 newPos = plane.target.gameObject.transform.position + plane.airplane.gameObject.transform.forward * 100 + UnityEngine.Random.onUnitSphere * 30;
        Vector3 newPos = plane2.gameObject.transform.position + plane2.gameObject.transform.forward * 150 + UnityEngine.Random.onUnitSphere * 30;
        //newPos.y = Mathf.Clamp(newPos.y, 20, 300);
        gameObject.transform.position = newPos;
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
