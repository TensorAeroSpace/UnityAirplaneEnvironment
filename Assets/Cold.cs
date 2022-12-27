using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : MonoBehaviour
{
    public AircraftManager aircraftManager;
    public float previousTime, interval;

    void FixedUpdate()
    {
        if (Time.time > previousTime + interval && Time.time < previousTime + 2 * interval)
        {
            aircraftManager.thrust = 0;
        } else if (Time.time > previousTime + 2 * interval)
        {
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
