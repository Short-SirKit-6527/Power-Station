using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class DozerController : NetworkBehaviour
{
    public bool Offline;
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public double maxMotorTorque; // maximum torque the motor can apply to wheel
    public double maxSteeringAngle; // maximum steer angle the wheel can have
    public double motorMaxRPM;
    public double steeringMaxRPM;
    public bool AI;
    public Camera cam;

    public double playerNum;

    public double WheelP;
    public double WheelI;
    public double WheelD;

    private double PIDRE;
    private double PIDRA;
    private double PIDLE;
    private double PIDLA;
    public void OnStartLocalPlayer()
    {
        cam.enabled = false;
    }
    public void FixedUpdate()
    {
        return;
        /**
        if (this.isLocalPlayer)
            {
            cam.enabled = true;
            cam.GetComponent<AudioListener>().enabled = true;
            double motor = maxMotorTorque * -1f * Input.GetAxis("Vertical");
            double motorRPM = motorMaxRPM * -1f * Input.GetAxis("Vertical");
            double steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            double steeringRPM = steeringMaxRPM * Input.GetAxis("Horizontal");

            int index = 1;



            double[] info = new double[4 * 4];
            info[0, 0] = motor;
            info[0, 1] = motorRPM;
            info[0, 2] = steering;
            info[0, 3] = steeringRPM;
            CmdMove(info);

            motor = info[0, 0];
            motorRPM = info[0, 1];
            steering = info[0, 2];
            steeringRPM = info[0, 3];
            index = 1;
            foreach (AxleInfo axleInfo in axleInfos)
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
                double leftPow = 0;
                double rightPow = 0;
                double leftRPM = 0;
                double rightRPM = 0;
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
                leftPow /= 2;
                rightPow /= 2;
                if (axleInfo.motor && axleInfo.steering)
                {
                    //leftPow /= 2;
                    //rightPow /= 2;
                    //leftRPM /= 2;
                    //rightRPM /= 2;
                }
                //AxleInfo axleInfo = axleInfos[index];
                /**
                if (axleInfo.leftWheel.rpm < leftRPM && leftRPM >= 0 || axleInfo.leftWheel.rpm > leftRPM && leftRPM <= 0) axleInfo.leftWheel.motorTorque = leftPow;
                else axleInfo.leftWheel.motorTorque = 0;
                if (axleInfo.rightWheel.rpm < rightRPM && rightRPM >= 0 || axleInfo.rightWheel.rpm > rightRPM && rightRPM <= 0) axleInfo.rightWheel.motorTorque = rightPow;
                else axleInfo.rightWheel.motorTorque = 0;

                if (leftPow == 0) axleInfo.leftWheel.brakeTorque = axleInfo.brakeTorque;
                else axleInfo.leftWheel.brakeTorque = 0;
                if (rightPow == 0) axleInfo.rightWheel.brakeTorque = axleInfo.brakeTorque;
                else axleInfo.rightWheel.brakeTorque = 0;
// **
                info[index, 0] = leftPow;
                info[index, 1] = leftRPM;
                info[index, 2] = rightPow;
                info[index, 3] = rightRPM;

            }
            index = 1;
            //Debug.Log("=======================");
            //Debug.Log(info[1, 1]);
            foreach (AxleInfo axleInfo in axleInfos)
            {
                double leftPow = info[index, 0];
                double leftRPM = info[index, 1];
                double rightPow = info[index, 2];
                double rightRPM = info[index, 3];
                //if (axleInfo.leftWheel.rpm < leftRPM && leftRPM >= 0 || axleInfo.leftWheel.rpm > leftRPM && leftRPM <= 0) axleInfo.leftWheel.motorTorque = leftPow;
                //else axleInfo.leftWheel.motorTorque = 0;
                //if (axleInfo.rightWheel.rpm < rightRPM && rightRPM >= 0 || axleInfo.rightWheel.rpm > rightRPM && rightRPM <= 0) axleInfo.rightWheel.motorTorque = rightPow;
                //else axleInfo.rightWheel.motorTorque = 0;
                //Debug.Log(axleInfo.leftWheel.rpm);
                //Debug.Log((leftRPM - axleInfo.leftWheel.rpm) * WheelP);
                //if (leftRPM >= 1000000) axleInfo.leftWheel.motorTorque = (leftRPM - axleInfo.leftWheel.rpm) * WheelP * -1;
                //else axleInfo.leftWheel.motorTorque = (leftRPM - axleInfo.leftWheel.rpm) * WheelP;
                PIDLA += leftRPM - axleInfo.leftWheel.rpm;
                double motorTorque = (leftRPM - axleInfo.leftWheel.rpm) * axleInfo.WheelP;
                if (Math.Abs(motorTorque) > 300 || Math.Abs(motorTorque) < 15)
                {
                    motorTorque = 0;
                    //axleInfo.leftWheel.brakeTorque = axleInfo.brakeTorque;
                }
                else axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.leftWheel.motorTorque = (float)motorTorque;// + PIDLA * WheelI + (PIDLE - leftRPM - axleInfo.leftWheel.rpm) * WheelD;
                PIDLE = leftRPM - axleInfo.leftWheel.rpm;
                //if (rightRPM >= 1000000) axleInfo.rightWheel.motorTorque = (rightRPM - axleInfo.rightWheel.rpm) * WheelP * -1;
                //else axleInfo.rightWheel.motorTorque = (rightRPM - axleInfo.rightWheel.rpm) * WheelP;
                PIDRA += rightRPM - axleInfo.rightWheel.rpm;
                motorTorque = (rightRPM - axleInfo.rightWheel.rpm) * axleInfo.WheelP;// + PIDRA * WheelI + (PIDRE - rightRPM - axleInfo.rightWheel.rpm) * WheelD;
                if (Math.Abs(motorTorque) > 300 || Math.Abs(motorTorque) < 15)
                {
                    motorTorque = 0;
                    //axleInfo.rightWheel.brakeTorque = axleInfo.brakeTorque;
                }
                else axleInfo.rightWheel.brakeTorque = 0;
                axleInfo.rightWheel.motorTorque = (float)motorTorque;
                PIDRE = rightRPM - axleInfo.rightWheel.rpm;
                //if (Math.Abs(motorTorque) < 0.01 && Math.Abs(axleInfo.rightWheel.rpm) < 1) axleInfo.rightWheel.brakeTorque = axleInfo.brakeTorque;
                //else axleInfo.rightWheel.brakeTorque = 0;

                /**
                if (rightPow == 0) axleInfo.rightWheel.brakeTorque = axleInfo.brakeTorque;
                else axleInfo.rightWheel.brakeTorque = 0;**
            }
        

        if (Input.GetAxis("Fire3") == 1)
            {
                transform.position = new Vector3(0, 1.12f, -7.4f);
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }
        **/
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.name == "Enemy")
        {
            //SendMessageUpwards("hitBySlash");
            //other.Death();
            //Destroy(other.gameObject);
            //var enemy: GameObject = other.transform.parent.gameObject;
            //var enemy: GameObject = other.transform.root.gameObject;
            //var enemy: GameObject = other.collider.gameObject;
            //other.Death();
            //enemy.BreadcastMessage("Death");
            //if (other.GetComponent(SentaEnemyShooter)){other.GetComponent(SentaEnemyShooter).Death();}
            //MyScript ms = other.collider.GetComponent();
            //ms.Death();
        }
    }

    [Command]
    public void CmdStop()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.brakeTorque = (float)axleInfo.brakeTorque;
            axleInfo.rightWheel.brakeTorque = (float)axleInfo.brakeTorque;
        }
    }

    /**
    [Command]
    public void CmdMove(double[,] info)
    {
        //Debug.Log(this);
        double motor = info[0, 0];
        double motorRPM = info[0, 1];
        double steering = info[0, 2];
        double steeringRPM = info[0, 3];
        int index = 1;
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.brakeTorque = 0;
            axleInfo.rightWheel.brakeTorque = 0;
            double leftPow = 0;
            double rightPow = 0;
            double leftRPM = 0;
            double rightRPM = 0;
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
            leftPow /= 2;
            rightPow /= 2;
            if (axleInfo.motor && axleInfo.steering)
            {
                //leftPow /= 2;
                //rightPow /= 2;
                //leftRPM /= 2;
                //rightRPM /= 2;
            }
            //AxleInfo axleInfo = axleInfos[index];
            /**
            if (axleInfo.leftWheel.rpm < leftRPM && leftRPM >= 0 || axleInfo.leftWheel.rpm > leftRPM && leftRPM <= 0) axleInfo.leftWheel.motorTorque = leftPow;
            else axleInfo.leftWheel.motorTorque = 0;
            if (axleInfo.rightWheel.rpm < rightRPM && rightRPM >= 0 || axleInfo.rightWheel.rpm > rightRPM && rightRPM <= 0) axleInfo.rightWheel.motorTorque = rightPow;
            else axleInfo.rightWheel.motorTorque = 0;

            if (leftPow == 0) axleInfo.leftWheel.brakeTorque = axleInfo.brakeTorque;
            else axleInfo.leftWheel.brakeTorque = 0;
            if (rightPow == 0) axleInfo.rightWheel.brakeTorque = axleInfo.brakeTorque;
            else axleInfo.rightWheel.brakeTorque = 0;
// **
            info[index, 0] = leftPow;
            info[index, 1] = leftRPM;
            info[index, 2] = rightPow;
            info[index, 3] = rightRPM;

        }
        index = 1;
        Debug.Log("=======================");
        Debug.Log(info[1, 1]);
        foreach (AxleInfo axleInfo in axleInfos)
        {
            double leftPow = info[index, 0];
            double leftRPM = info[index, 1];
            double rightPow = info[index, 2];
            double rightRPM = info[index, 3];
            //if (axleInfo.leftWheel.rpm < leftRPM && leftRPM >= 0 || axleInfo.leftWheel.rpm > leftRPM && leftRPM <= 0) axleInfo.leftWheel.motorTorque = leftPow;
            //else axleInfo.leftWheel.motorTorque = 0;
            //if (axleInfo.rightWheel.rpm < rightRPM && rightRPM >= 0 || axleInfo.rightWheel.rpm > rightRPM && rightRPM <= 0) axleInfo.rightWheel.motorTorque = rightPow;
            //else axleInfo.rightWheel.motorTorque = 0;
            //Debug.Log(axleInfo.leftWheel.rpm);
            Debug.Log((leftRPM - axleInfo.leftWheel.rpm) * WheelP);
            //if (leftRPM >= 1000000) axleInfo.leftWheel.motorTorque = (leftRPM - axleInfo.leftWheel.rpm) * WheelP * -1;
            //else axleInfo.leftWheel.motorTorque = (leftRPM - axleInfo.leftWheel.rpm) * WheelP;
            PIDLA += leftRPM - axleInfo.leftWheel.rpm;
            double motorTorque = (leftRPM - axleInfo.leftWheel.rpm) * axleInfo.WheelP;
            if (Math.Abs(motorTorque) > 300 || Math.Abs(motorTorque) < 15)
            {
                motorTorque = 0;
                //axleInfo.leftWheel.brakeTorque = axleInfo.brakeTorque;
            }
            else axleInfo.leftWheel.brakeTorque = 0;
            axleInfo.leftWheel.motorTorque = (float)motorTorque;// + PIDLA * WheelI + (PIDLE - leftRPM - axleInfo.leftWheel.rpm) * WheelD;
            PIDLE = leftRPM - axleInfo.leftWheel.rpm;
            //if (rightRPM >= 1000000) axleInfo.rightWheel.motorTorque = (rightRPM - axleInfo.rightWheel.rpm) * WheelP * -1;
            //else axleInfo.rightWheel.motorTorque = (rightRPM - axleInfo.rightWheel.rpm) * WheelP;
            PIDRA += rightRPM - axleInfo.rightWheel.rpm;
            motorTorque = (rightRPM - axleInfo.rightWheel.rpm) * axleInfo.WheelP;// + PIDRA * WheelI + (PIDRE - rightRPM - axleInfo.rightWheel.rpm) * WheelD;
            if (Math.Abs(motorTorque) > 300 || Math.Abs(motorTorque) < 15)
            {
                motorTorque = 0;
                //axleInfo.rightWheel.brakeTorque = axleInfo.brakeTorque;
            }
            else axleInfo.rightWheel.brakeTorque = 0;
            axleInfo.rightWheel.motorTorque = (float)motorTorque;
            PIDRE = rightRPM - axleInfo.rightWheel.rpm;

            /**
            if (leftPow == 0) axleInfo.leftWheel.brakeTorque = axleInfo.brakeTorque;
            else axleInfo.leftWheel.brakeTorque = 0;
            if (rightPow == 0) axleInfo.rightWheel.brakeTorque = axleInfo.brakeTorque;
            else axleInfo.rightWheel.brakeTorque = 0;**
        }
    }**/
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
    public double brakeTorque;

    public double WheelP;
    public double WheelI;
    public double Wheeld;
}