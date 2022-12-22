using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class PlaneAgent : Agent
{
    /// <summary>
    /// Target object that plane tries to reach 
    /// </summary>
    public GameObject target;
    
    public float rotation_up, rotation_strafe, rotation_angle, speed;
    
    /// <inheritdoc/>
    /// [Observations and Sensors]: https://github.com/Unity-Technologies/ml-agents/blob/release_20_docs/docs/Learning-Environment-Design-Agents.md#observations-and-sensors
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(gameObject.transform.rotation.z);
        sensor.AddObservation(gameObject.transform.rotation.y);
        sensor.AddObservation(gameObject.transform.rotation.x);
        sensor.AddObservation(target.transform.position - gameObject.transform.position);
    }

    /// <inheritdoc/>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var up = actionBuffers.DiscreteActions[0] - 1;
        var strafe = actionBuffers.DiscreteActions[1] - 1;
        var angle = actionBuffers.DiscreteActions[2] - 1;

        gameObject.transform.Rotate(up * rotation_up, strafe * rotation_strafe, angle * rotation_angle);

        gameObject.transform.position += gameObject.transform.forward * speed;
        SetReward(-1f);
    }

    /// <inheritdoc/>>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        discreteActionsOut[1] = 0;
        discreteActionsOut[2] = 0;
    }

    /// <summary>
    /// Method for setting reward to model
    /// </summary>
    public void Win()
    {
        SetReward(100f);
    }
}
