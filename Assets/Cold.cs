using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : MonoBehaviour
{
    public AircraftManager aircraftManager;
    public float previousTime, interval;
    public GameObject controlsFrozen;

    void FixedUpdate()
    {
        if (Time.time > previousTime + interval && Time.time < previousTime + 2 * interval)
        {
            aircraftManager.thrust = 0;
            aircraftManager.frozen = true;
            controlsFrozen.SetActive(true);
        } else if (Time.time > previousTime + 2 * interval)
        {
            aircraftManager.frozen = false;
            previousTime = Time.time;
            controlsFrozen.SetActive(false);
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
