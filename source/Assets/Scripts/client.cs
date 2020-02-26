using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;

public class client : NetworkedBehaviour
{
    // Start is called before the first frame update

    private NetworkedClient nc;
    void Start()
    {
        nc = new NetworkedClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerRPC]
    public int getAssignment()
    {
        int assignment = -1;

        return assignment;
    }
}
