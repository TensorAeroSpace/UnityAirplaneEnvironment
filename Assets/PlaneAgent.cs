using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class PlaneAgent : Agent
{
    public GameObject target;
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(gameObject.transform.rotation.z);
        sensor.AddObservation(gameObject.transform.rotation.y);
        sensor.AddObservation(gameObject.transform.rotation.x);
        sensor.AddObservation(target.transform.position - gameObject.transform.position);
    }

    public float rotation_up, rotation_strafe, rotation_angle, speed;

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var up = actionBuffers.DiscreteActions[0] - 1;
        var strafe = actionBuffers.DiscreteActions[1] - 1;
        var angle = actionBuffers.DiscreteActions[2] - 1;

        gameObject.transform.Rotate((float)up * rotation_up, (float)strafe * rotation_strafe, (float)angle * rotation_angle);

        gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * speed;
        SetReward(-1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        discreteActionsOut[1] = 0;
        discreteActionsOut[2] = 0;
    }

    public void Win()
    {
        SetReward(100f);
    }
}
