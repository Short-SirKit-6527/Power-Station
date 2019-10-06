﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DozerControllerOffline : MonoBehaviour
{
    public bool Offline;
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float motorMaxRPM;
    public float steeringMaxRPM;
    public bool AI;
    public Camera cam;

    public void FixedUpdate()
    {

        if (AI)
        {

        }
        else
        {
            cam.enabled = true;
            float motor = maxMotorTorque * -1f * Input.GetAxis("Vertical");
            float motorRPM = motorMaxRPM * -1f * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            float steeringRPM = steeringMaxRPM * Input.GetAxis("Horizontal");

            foreach (AxleInfo axleInfo in axleInfos)
            {
                float leftPow = 0f;
                float rightPow = 0f;
                float leftRPM = 0f;
                float rightRPM = 0f;
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
                if (axleInfo.leftWheel.rpm < leftRPM && leftRPM >= 0 || axleInfo.leftWheel.rpm > leftRPM && leftRPM <= 0) axleInfo.leftWheel.motorTorque = leftPow;
                else axleInfo.leftWheel.motorTorque = 0f;
                if (axleInfo.rightWheel.rpm < rightRPM && rightRPM >= 0 || axleInfo.rightWheel.rpm > rightRPM && rightRPM <= 0) axleInfo.rightWheel.motorTorque = rightPow;
                else axleInfo.rightWheel.motorTorque = 0f;

                if (leftPow == 0) axleInfo.leftWheel.brakeTorque = axleInfo.brakeTorque;
                else axleInfo.leftWheel.brakeTorque = 0f;
                if (rightPow == 0) axleInfo.rightWheel.brakeTorque = axleInfo.brakeTorque;
                else axleInfo.rightWheel.brakeTorque = 0f;

                Debug.Log(leftRPM);
            }

            if (Input.GetAxis("Fire3") == 1)
            {
                transform.position = new Vector3(0f, 0.798f, -7.4f);
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }

            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }
        if (Input.GetKey("1")) QualitySettings.SetQualityLevel(0, true);
        if (Input.GetKey("2")) QualitySettings.SetQualityLevel(1, true);
        if (Input.GetKey("3")) QualitySettings.SetQualityLevel(2, true);
        if (Input.GetKey("4")) QualitySettings.SetQualityLevel(3, true);
        if (Input.GetKey("5")) QualitySettings.SetQualityLevel(4, true);
        if (Input.GetKey("6")) QualitySettings.SetQualityLevel(5, true);
        if (Input.GetKey("7")) QualitySettings.SetQualityLevel(6, true);
        if (Input.GetKey("8")) QualitySettings.SetQualityLevel(7, true);
        if (Input.GetKey("9")) QualitySettings.SetQualityLevel(8, true);
        if (Input.GetKey("0")) QualitySettings.SetQualityLevel(9, true);
    }
}