using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public List<ControlSystem> controlSources;
    public VariableJoystick touchJoystick;
    // Start is called before the first frame update
    void Start()
    {
        ControlSystem.touchJoystick = touchJoystick;
        controlSources[0].setInputScheme((InputMode) MenuScript.inputSchemeIndex);
        touchJoystick.gameObject.SetActive((InputMode)MenuScript.inputSchemeIndex == InputMode.TouchScreen);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (ControlSystem controlSource in controlSources)
        {
            controlSource.loopTask();
        }
    }
}

[System.Serializable]
public class ControlSystem
{
    public InputMode inputMode;
    public int index;
    public GameObject associatedObject;
    public static VariableJoystick touchJoystick;

    public void setInputScheme(InputMode mode)
    {
        inputMode = mode;
    }

    public void loopTask()
    {
        double[] inputs = GetMovement();
        RobotController associatedController = associatedObject.GetComponent(typeof(RobotController)) as RobotController;
        associatedController.drive(inputs[0], inputs[1]);
    }

    public double[] GetMovement()
    {
        double[] val = new double[] { 0f, 0f };
        switch (inputMode)
        {
            case InputMode.Autonomous:
                val[0] = 0;
                val[1] = 0;
                break;
            case InputMode.Standard:
                val[0] = Input.GetAxis("Vertical");
                val[1] = Input.GetAxis("Horizontal");
                break;
            case InputMode.TouchScreen:
                val[0] = touchJoystick.Vertical;
                val[1] = touchJoystick.Horizontal;
                break;
            case InputMode.UnetClient:
                val[0] = 0;
                val[1] = 0;
                break;
            case InputMode.UnetServer:
                val[0] = 0;
                val[1] = 0;
                break;
            default:
                val[0] = 0;
                val[1] = 0;
                break;
        }
        return val;
    }
}

public enum InputMode
{
    Autonomous = 0,
    Standard = 1,
    TouchScreen = 2,
    UnetClient = 3,
    UnetServer = 4
}

public class TILMDA //Time Indexed Looping Multi Dimensional Array
{
    public double[] data;
    public double[] indexes;
    public int timeStartIndex;
    public int dimensions;

    public TILMDA(int dataLength, int dimensionCount)
    {
        dimensions = dimensionCount;
        data = new double[dataLength * dimensionCount];
        for (int indexT = 0; indexT < dataLength; indexT++)
        {
            indexes[indexT] = 0;
            for (int indexD = 0; indexD < dimensionCount; indexD++)
            {
                data[indexT * dimensionCount + indexD] = 0d;
            }
        }
        timeStartIndex = 0;
    }

    public bool AppendValue(double[] values)
    {
        return SetValuesAt(values, Time.timeSinceLevelLoad);
    }

    public bool SetValuesAt(double[] values, double time)
    {
        if (time > indexes[timeStartIndex])
        {
            indexes[timeStartIndex] = time;
            for (int indexD = 0; indexD < dimensions; indexD++)
            {
                //data[timeStartIndex, indexD] = values[indexD];
            }
            timeStartIndex = (timeStartIndex + 1) % indexes.Length;
        }
        else
        {
            return false;
        }
        return true;
    }

    public double[] GetValueAt(double time)
    {
        double[] value = new double[dimensions];
        int finalIndex = timeStartIndex;
        do
        {
            finalIndex++;
        }
        while (indexes[(finalIndex % indexes.Length)] < time || (finalIndex % indexes.Length) == timeStartIndex);
        finalIndex = finalIndex % indexes.Length;


        if (indexes[finalIndex] == time)
        {
            for (int indexD = 0; indexD < dimensions; indexD++){
                //value[indexD] = data[finalIndex, indexD];
            }
        }
        else
        {
            int initialIndex = (finalIndex - 1) % indexes.Length;
            double interpolationRatio = (time - indexes[initialIndex]) / (indexes[finalIndex] - indexes[initialIndex]);
            for (int indexD = 0; indexD < dimensions; indexD++)
            {
                //value[indexD] = (data[initialIndex, indexD] - data[finalIndex, indexD]) * interpolationRatio + data[initialIndex, indexD];
            }
        }

        return value;
    }
}

public class RobotController : NetworkedBehaviour
{
    public virtual void drive(double vertical, double horizontal)
    {
        Debug.Log("Generic RobotController drive function. Override me!");
    }
}