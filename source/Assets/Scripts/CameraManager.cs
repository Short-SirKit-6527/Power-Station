using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraManager : MonoBehaviour
{

    public Transform target;
    public float distance;
    public float height;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time / 4f) * distance, height, Mathf.Cos(Time.time / 4f) * distance);
        transform.LookAt(target);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
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
