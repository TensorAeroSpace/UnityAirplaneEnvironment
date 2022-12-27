using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birds : MonoBehaviour
{
    public Rigidbody aircraftRigidBody;
    public float previousTime, interval, impact, startHeight;
    public GameObject bird, wing1, wing2, wing3;

    public int currentPart;

    void FixedUpdate()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        previousTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > previousTime + interval)
        {
            if (currentPart == 0)
            {
                aircraftRigidBody.AddForce(new Vector3(1, 0, 0) * Random.Range(impact, 2 * impact));
            }
            else if (currentPart == 1)
            {
                aircraftRigidBody.AddForce(new Vector3(-1, 0, 0) * Random.Range(impact, 2 * impact));
            }
            else
            {
                aircraftRigidBody.AddForce(new Vector3(0, -1, 0) * Random.Range(impact, 2 * impact));
            }
            previousTime = Time.time;

            currentPart = Random.Range(0, 3);

            if (currentPart == 0)
            {
                bird.transform.position = wing1.transform.position + new Vector3(0, 0, -startHeight);
            }
            else if (currentPart == 1)
            {
                bird.transform.position = wing2.transform.position + new Vector3(0, 0, -startHeight);
            } 
            else
            {

                bird.transform.position = wing3.transform.position + new Vector3(0, 0, -startHeight);
            }
        }
        else
        {

            if (currentPart == 0)
            {
                bird.transform.position = wing1.transform.position + new Vector3(0, 0, -(startHeight - startHeight / interval * (Time.time - previousTime)));
            }
            else if (currentPart == 1)
            {
                bird.transform.position = wing2.transform.position + new Vector3(0, 0, -(startHeight - startHeight / interval * (Time.time - previousTime)));
            }
            else
            {
                bird.transform.position = wing3.transform.position + new Vector3(0, 0, -(startHeight - startHeight / interval * (Time.time - previousTime)));
            }
        }
    }
}
