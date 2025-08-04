using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class FlyingAgent : Agent
{
    public GameObject target;
    public GameObject airplane;
    public AircraftManager manager;
    public MainMFD canvas;
    public FlightDynamicsLabManager more_manager;
    
    public override void OnEpisodeBegin()
    {
        more_manager.DoExperimentSetup();
        manager.ResetInitialize();
        target.GetComponent<TargetCube>().ResetPos();
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(airplane.transform.rotation.z);
        sensor.AddObservation(airplane.transform.rotation.y);
        sensor.AddObservation(airplane.transform.rotation.x);
        sensor.AddObservation(target.transform.position - airplane.transform.position);
        //sensor.AddObservation(target.transform.position.y);
        sensor.AddObservation(0);
        sensor.AddObservation(manager.flapAngle);
        sensor.AddObservation(manager.flapTarget);
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

        Vector3 my_dir = airplane.transform.forward;
        Vector3 target_dir = target.transform.position - airplane.transform.position;
        target_dir /= target_dir.magnitude;
        float weight = 1f;
        Vector3 project_dir = Vector3.Project(my_dir, target_dir);
        if (Vector3.Angle(project_dir, target_dir) > 1f)
        {
            weight = -1f;
        }
        SetReward(Vector3.Project(my_dir, target_dir).magnitude * weight);
        
        //Debug.Log(Vector3.Project(my_dir, target_dir).magnitude * weight);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.ContinuousActions;
        ///*
        discreteActionsOut[0] = Input.GetAxis("Thrust");
        discreteActionsOut[1] = Input.GetAxis("Aileron");
        discreteActionsOut[2] = Input.GetAxis("Elevator");
        //Debug.Log(Input.GetAxis("Elevator"));
        discreteActionsOut[3] = Input.GetAxis("Rudder");
        discreteActionsOut[4] = Input.GetAxis("Elevator Trim");
        discreteActionsOut[5] = Input.GetAxis("FlapDown");
        discreteActionsOut[6] = Input.GetAxis("FlapUp");//*/
        /*
        discreteActionsOut[0] = 1f;
        discreteActionsOut[1] = 0f;
        discreteActionsOut[2] = -0.7f;
        discreteActionsOut[3] = 0f;
        discreteActionsOut[4] = 0f;
        discreteActionsOut[5] = 1f;
        discreteActionsOut[6] = 0f;*/
        
    }

    public void Win()
    {
        SetReward(1000f);
    }
}
