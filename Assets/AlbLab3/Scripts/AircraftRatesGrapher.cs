using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftRatesGrapher : MonoBehaviour
{
    // Quick script just to log things to Grapher
    FlightDynamicsLabManager Manager { get { return FlightDynamicsLabManager.Singleton(); } }

    void FixedUpdate()
    {
        Vector3 localAngularVelocity = Manager.aircraftRb.transform.InverseTransformDirection(Manager.aircraftRb.angularVelocity);
        Vector3 localAngularVelocity_aircraftAxes = CoordinateTransform.UnityToAircraftMoment(localAngularVelocity);
    }
}
