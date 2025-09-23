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
        prev_actions = new float[7];
        for(int i = 0; i < 7; i++) {
            prev_actions[i] = 0f;
        }
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(airplane.transform.rotation.z);
        sensor.AddObservation(airplane.transform.rotation.y);
        sensor.AddObservation(airplane.transform.rotation.x);
        sensor.AddObservation(target.transform.position - airplane.transform.position);
        //sensor.AddObservation(target.transform.position.y);
        sensor.AddObservation(0);
        sensor.AddObservation(manager.flapAngle / 180.0F);
        sensor.AddObservation(manager.flapTarget / 180.0F);
        sensor.AddObservation(canvas.gearDown);

        sensor.AddObservation(canvas.speed / 10.0F);
        //sensor.AddObservation(canvas.altitude);
        sensor.AddObservation(canvas.pitch / 180.0F);
        sensor.AddObservation(canvas.roll / 180.0F);
        sensor.AddObservation(canvas.heading / 10.0F);
        sensor.AddObservation(canvas.turnRate / 10.0F);
        sensor.AddObservation(canvas.gForce / 10.0F);
        sensor.AddObservation(canvas.maxGForce / 10.0F);
        sensor.AddObservation(canvas.minGForce / 10.0F);
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
/*
        w_progress: float = 5.0        # прогресс по расстоянию
        w_vel_towards: float = 1.0     # проекция скорости на вектор к цели
        w_heading_align: float = 0.5   # выравнивание курса на цель
        w_altitude_band: float = 0.2   # штраф за выход из коридора высот
        w_attitude: float = 0.05       # штраф за большие углы (крен/тангаж)
        w_ctrl_smooth: float = 0.02    # штраф за резкие изменения управляющих
        w_step: float = 0.01           # шаговый штраф за время/топливо
        
        # события
        r_success: float = 1000.0
        r_crash: float = -1000.0
        r_out_of_bounds: float = -500.0
        
        # доп. параметры
        goal_radius: float = 30.0      # радиус засчитывания цели (м)
        min_alt: float = 20.0          # минимальная безопасная высота (м)
        max_alt: float = 3000.0        # максимальная высота (м)
        desired_alt: float = 300.0     # целевая высота (для “коридора”)
        alt_band: float = 100.0        # ширина комфортного коридора (±)
        max_bank_deg: float = 60.0     # допустимый крен
        max_pitch_deg: float = 30.0    # допустимый тангаж*/

        float w_progress = 5.0f;

        float new_dist = (airplane.transform.position - target.transform.position).magnitude;

        float r_progress = prev_dist - new_dist;

        Vector3 target_dir = target.transform.position - airplane.transform.position;
        target_dir /= target_dir.magnitude;

        float w_vel_towards = 1.0f;

        Vector3 vel = airplane.GetComponent<Rigidbody>().velocity;
        vel /= vel.magnitude;

        Vector3 proj_vel = Vector3.Project(vel, target_dir);

        float r_vel_towards = proj_vel.magnitude;

        if (Vector3.Angle(proj_vel, vel) > 1f)
        {
            r_vel_towards *= -1.0f;
        }

        float w_heading_align = 0.5f;

        Vector3 my_dir = airplane.transform.forward;
        float weight = 1f;
        Vector3 project_dir = Vector3.Project(my_dir, target_dir);
        if (Vector3.Angle(project_dir, target_dir) > 1f)
        {
            weight = -1f;
        }

        float r_heading_align = Vector3.Project(my_dir, target_dir).magnitude * weight;

        float w_ctrl_smooth = 0.02f;

        float r_ctrl_smooth = 0f;

        for(int i = 0; i < 7; i++) {
            r_ctrl_smooth += Mathf.Abs(prev_actions[i] - actionBuffers.ContinuousActions[i]);
        }

        r_ctrl_smooth /= 7;

        float w_altitude_band= 0.2f;

        float r_altitude_band = 0f;

        if(Mathf.Abs(airplane.transform.position.y - 10000f) > 50f) {
            r_altitude_band =-1f;
        }

        float w_attitude = 0.05f;

        float r_attitude=0f;

        if(Mathf.Abs(canvas.pitch) > 30.0f|| Mathf.Abs(canvas.roll) > 60.0f) {
            r_attitude=-1f;
        }

        float w_step= 0.01f;

        SetReward(w_vel_towards * r_vel_towards + w_heading_align * r_heading_align - w_ctrl_smooth * r_ctrl_smooth + w_altitude_band * r_altitude_band +
        w_attitude * r_attitude - w_step);
        
        //Debug.Log(Vector3.Project(my_dir, target_dir).magnitude * weight);
        prev_dist = new_dist;

        for(int i = 0; i < 7; i++) {
            prev_actions[i]= actionBuffers.ContinuousActions[i];
        }
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
