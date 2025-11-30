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
    public float prev_dist;
    public float[] prev_actions;
    
    public override void OnEpisodeBegin()
    {
        more_manager.DoExperimentSetup();
        manager.ResetInitialize();
        target.GetComponent<TargetCube>().ResetPos();
        prev_dist = (airplane.transform.position - target.transform.position).magnitude;
        prev_actions = new float[4];
        for(int i = 0; i < 4; i++) {
            prev_actions[i] = 0f;
        }
    }
    
    public override void CollectObservations(VectorSensor sensor)
    { /*
        Debug.Log("==============================");
        Debug.Log(airplane.transform.rotation.z);
        Debug.Log(airplane.transform.rotation.y);
        Debug.Log(airplane.transform.rotation.x);
        Debug.Log(target.transform.position - airplane.transform.position);
        Debug.Log(manager.flapAngle);
        Debug.Log(manager.flapTarget);
        Debug.Log(canvas.gearDown);
        Debug.Log(canvas.pitch);
        Debug.Log(canvas.roll);
        Debug.Log(canvas.heading);
        Debug.Log(canvas.turnRate);
        Debug.Log(canvas.gForce);
        Debug.Log(canvas.maxGForce);
        Debug.Log(canvas.minGForce);
        Debug.Log(canvas.alpha);
        Debug.Log(canvas.beta);
        Debug.Log(canvas.vv);
        Debug.Log(canvas.hv);
        Debug.Log(canvas.engineTarget);
        Debug.Log(canvas.engine);
        Debug.Log(canvas.fuelTarget);
        Debug.Log(canvas.fuel);
        Debug.Log(canvas.fuelFlow);
        Debug.Log(canvas.temperatureTarget);
        Debug.Log(canvas.temperature); */
        sensor.AddObservation(airplane.transform.rotation.z);
        sensor.AddObservation(airplane.transform.rotation.y);
        sensor.AddObservation(airplane.transform.rotation.x);
        Vector3 diff = target.transform.position - airplane.transform.position;
        sensor.AddObservation(diff); // как есть

        Vector3 dirNorm = diff.normalized;
        sensor.AddObservation(dirNorm); // три доп. компоненты
        //sensor.AddObservation(target.transform.position.y);
        //sensor.AddObservation(0);
        sensor.AddObservation(manager.flapAngle);
        sensor.AddObservation(manager.flapTarget);
        sensor.AddObservation(canvas.gearDown);

        //Debug.Log(canvas.speed);

        sensor.AddObservation(canvas.speed);
        //sensor.AddObservation(canvas.altitude);
        sensor.AddObservation(canvas.pitch);
        sensor.AddObservation(canvas.roll);
        sensor.AddObservation(canvas.heading / 180f);
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
        var elevatorTrimAction = 0f;//actionBuffers.ContinuousActions[4];
        var flapDownAction = 0f;//actionBuffers.ContinuousActions[5];
        var flapUpAction = 0f;//actionBuffers.ContinuousActions[6];

        //Debug.Log(actionBuffers.ContinuousActions[0]);
        //Debug.Log(actionBuffers.ContinuousActions[1]);
        //Debug.Log(actionBuffers.ContinuousActions[2]);
        //Debug.Log(actionBuffers.ContinuousActions[3]);
        //Debug.Log(actionBuffers.ContinuousActions[4]);
        //Debug.Log(actionBuffers.ContinuousActions[5]);
        //Debug.Log(actionBuffers.ContinuousActions[6]);

        gameObject.GetComponent<AircraftManager>().SetControlInputs(thrustAction, aileronAction,  elevatorAction, rudderAction,  elevatorTrimAction, flapDownAction, flapUpAction);
        // веса
        /*
        float w_progress      = 0.5f;
        float w_vel_towards   = 0.2f;
        float w_heading_align = 0.1f;
        float w_ctrl_smooth   = 0.01f;
        float w_altitude_band = 0.1f;
        float w_attitude      = 0.1f;
        float w_step          = 0.001f;

        // 1) Прогресс по расстоянию
        float new_dist = (airplane.transform.position - target.transform.position).magnitude;
        float r_progress = prev_dist - new_dist; // >0 если приближаемся
        AddReward(w_progress * r_progress);

        // 2) Скорость в сторону цели
        Vector3 target_dir = (target.transform.position - airplane.transform.position).normalized;
        Vector3 vel = airplane.GetComponent<Rigidbody>().velocity;
        if (vel.sqrMagnitude > 1e-4f)
        {
            Vector3 velNorm = vel.normalized;
            float dot = Vector3.Dot(velNorm, target_dir); // [-1,1]
            AddReward(w_vel_towards * dot);
        }

        // 3) Выравнивание носа самолёта на цель
        Vector3 my_dir = airplane.transform.forward.normalized;
        float headingDot = Vector3.Dot(my_dir, target_dir); // [-1,1]
        AddReward(w_heading_align * headingDot);

        // 4) Плавность управления
        float r_ctrl_smooth = 0f;
        for (int i = 0; i < 4; i++)
        {
            float diff = prev_actions[i] - actionBuffers.ContinuousActions[i];
            r_ctrl_smooth += Mathf.Abs(diff);
        }
        r_ctrl_smooth /= 4f;
        AddReward(-w_ctrl_smooth * r_ctrl_smooth);

        // 5) Коридор высоты (пример: вокруг 10000, ±150)
        float alt = airplane.transform.position.y;
        if (Mathf.Abs(alt - 10000f) > 150f)
        {
            AddReward(-w_altitude_band);
        }

        // 6) Ограничение по крену/тангажу
        if (Mathf.Abs(canvas.pitch) > 30.0f || Mathf.Abs(canvas.roll) > 60.0f)
        {
            AddReward(-w_attitude);
        }

        // 7) Шаговый штраф
        AddReward(-w_step);

        // обновление prev_dist и prev_actions
        prev_dist = new_dist;
        for (int i = 0; i < 4; i++)
        {
            prev_actions[i] = actionBuffers.ContinuousActions[i];
        }
        */

        if (MaxStep > 0 && StepCount >= MaxStep - 1)
        {
            //endedByTimeout = true;

            float dist = Vector3.Distance(airplane.transform.position, target.transform.position);
            float penalty = Mathf.Clamp(dist / 50f, 0f, 5f);
            AddReward(-penalty);

            EndEpisode();
            return;
        }

        

        float maxPitch = 40f;
        float maxRoll  = 70f;

        if (Mathf.Abs(canvas.pitch) > maxPitch || Mathf.Abs(canvas.roll) > maxRoll)
        {
            AddReward(-20f);
            //EndEpisode();
            return;
        }

        float wProgress = 1.0f;
        float wStep     = 0.0005f;


        float newDist = Vector3.Distance(airplane.transform.position, target.transform.position);
        float rProgress = prev_dist - newDist; // >0 если приближаешься
        AddReward(wProgress * rProgress);
        AddReward(-wStep);
        prev_dist = newDist;

        //Debug.Log(rProgress);

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
        //discreteActionsOut[4] = Input.GetAxis("Elevator Trim");
        //discreteActionsOut[5] = Input.GetAxis("FlapDown");
        //discreteActionsOut[6] = Input.GetAxis("FlapUp");//*/
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
        AddReward(50f);
        EndEpisode();
    }

    public void Timeout()
    {
        float dist = Vector3.Distance(airplane.transform.position, target.transform.position);
        float penalty = Mathf.Clamp(dist / 50f, 0f, 5f); // максимум -5
        AddReward(-penalty);
    }
}
