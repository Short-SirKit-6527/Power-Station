using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float state = Mathf.PingPong(Time.time / 4, 1);
        Color emissiveColor = new Color(state, 0.0f, state);
        rend.material.SetColor("_EmissionColor", emissiveColor);
    }
}
