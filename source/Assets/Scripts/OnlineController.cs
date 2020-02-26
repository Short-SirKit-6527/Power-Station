using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlineController : NetworkBehaviour
{
    public class inputs : SyncListStruct<float> { };
    public int playerNum;
    // Start is called before the first frame update
    void Start()
    {
        playerNum = (int)System.Math.Round(GetComponent<Transform>().position.x);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Transform>().position = new Vector3(playerNum + Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));

    }
}
