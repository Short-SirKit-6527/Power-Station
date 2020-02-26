using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DozerControllerOffline : RobotController
{
    public bool Offline;
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public double maxMotorTorque; // maximum torque the motor can apply to wheel
    public double maxSteeringAngle; // maximum steer angle the wheel can have
    public double motorMaxRPM;
    public double steeringMaxRPM;
    public bool AI;
    public Camera cam;

    public override void drive(double vertical, double horizontal)
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Menu");
        }
        if (AI)
        {

        }
        else
        {
            cam.enabled = true;
            double motor = maxMotorTorque * -1f * vertical;
            double motorRPM = motorMaxRPM * -1f * vertical;
            double steering = maxSteeringAngle * horizontal;
            double steeringRPM = steeringMaxRPM * horizontal;

            foreach (AxleInfo axleInfo in axleInfos)
            {
                double leftPow = 0f;
                double rightPow = 0f;
                double leftRPM = 0f;
                double rightRPM = 0f;
                if (axleInfo.motor)
                {
                    leftPow += motor;
                    rightPow += motor;
                    leftRPM += motorRPM;
                    rightRPM += motorRPM;
                    //axleInfo.leftWheel.steerAngle = steering;
                    //axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.steering)
                {
                    leftPow -= steering;
                    rightPow += steering;
                    leftRPM -= steeringRPM;
                    rightRPM += steeringRPM;
                    //axleInfo.leftWheel.motorTorque = motor;
                    //axleInfo.rightWheel.motorTorque = motor;
                }
                if (axleInfo.motor && axleInfo.steering)
                {
                    leftPow /= 2;
                    rightPow /= 2;
                    //leftRPM /= 2;
                    //rightRPM /= 2;
                }
                if (axleInfo.leftWheel.rpm < leftRPM && leftRPM >= 0 || axleInfo.leftWheel.rpm > leftRPM && leftRPM <= 0) axleInfo.leftWheel.motorTorque = (float)leftPow;
                else axleInfo.leftWheel.motorTorque = 0f;
                if (axleInfo.rightWheel.rpm < rightRPM && rightRPM >= 0 || axleInfo.rightWheel.rpm > rightRPM && rightRPM <= 0) axleInfo.rightWheel.motorTorque = (float)rightPow;
                else axleInfo.rightWheel.motorTorque = 0f;

                if (leftPow == 0) axleInfo.leftWheel.brakeTorque = (float)axleInfo.brakeTorque;
                else axleInfo.leftWheel.brakeTorque = 0f;
                if (rightPow == 0) axleInfo.rightWheel.brakeTorque = (float)axleInfo.brakeTorque;
                else axleInfo.rightWheel.brakeTorque = 0f;

            }

            if (Input.GetAxis("Fire3") == 1)
            {
                transform.position = new Vector3(0f, 1.12f, -7.4f);
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
    }
}
