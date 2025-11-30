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
        Vector3 forw_vec = plane2.gameObject.transform.forward;
        forw_vec.y = 0f;
        forw_vec /= forw_vec.magnitude;
        Vector3 newPos = plane2.gameObject.transform.position + forw_vec * 50 + UnityEngine.Random.onUnitSphere * 10;
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
        Debug.Log("Win!");
        plane.Win();
        //ResetPos();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
