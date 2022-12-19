using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class FlyingAgent : Agent
{
    public GameObject target;
    public GameObject airplane;
    public MainMFD canvas;
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(airplane.transform.rotation.z);
        sensor.AddObservation(airplane.transform.rotation.y);
        sensor.AddObservation(airplane.transform.rotation.x);
        sensor.AddObservation(target.transform.position - airplane.transform.position);
        sensor.AddObservation(target.transform.position.y);
        sensor.AddObservation(canvas.flapsIndex);
        sensor.AddObservation(canvas.currentFlap);
        sensor.AddObservation(canvas.gearDown);

        sensor.AddObservation(canvas.speed);
        sensor.AddObservation(canvas.altitude);
        sensor.AddObservation(canvas.pitch);
        sensor.AddObservation(canvas.roll);
        sensor.AddObservation(canvas.heading);
        sensor.AddObservation(canvas.turnRate);
        sensor.AddObservation(canvas.gForce);
        sensor.AddObservation(canvas.maxGForce);
        sensor.AddObservation(canvas.minGForce);
        sensor.AddObservation(canvas.alpha);
        sensor.AddObservation(canvas.beta);
        sensor.AddObservation(canvas.vv);
        sensor.AddObservation(canvas.hv);

        sensor.AddObservation(canvas.engineTarget); sensor.AddObservation(canvas.engine);
        sensor.AddObservation(canvas.fuelTarget); sensor.AddObservation(canvas.fuel);
        sensor.AddObservation(canvas.fuelFlow);
        sensor.AddObservation(canvas.temperatureTarget); sensor.AddObservation(canvas.temperature);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var thrustAction = actionBuffers.ContinuousActions[0];
        var aileronAction = actionBuffers.ContinuousActions[1];
        var elevatorAction = actionBuffers.ContinuousActions[2];
        var rudderAction = actionBuffers.ContinuousActions[3];
        var elevatorTrimAction = actionBuffers.ContinuousActions[4];
        var flapDownAction = actionBuffers.ContinuousActions[5];
        var flapUpAction = actionBuffers.ContinuousActions[6];

        gameObject.GetComponent<AircraftManager>().SetControlInputs(thrustAction, aileronAction,  elevatorAction, rudderAction,  elevatorTrimAction, flapDownAction, flapUpAction);

        SetReward(-1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.ContinuousActions;
        discreteActionsOut[0] = 1;
        discreteActionsOut[1] = 0;
        discreteActionsOut[2] = 0;
        discreteActionsOut[3] = 0;
        discreteActionsOut[4] = 0;
        discreteActionsOut[5] = 0;
        discreteActionsOut[6] = 0;
    }

    public void Win()
    {
        SetReward(100f);
    }
}
