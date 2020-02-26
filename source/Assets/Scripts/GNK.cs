using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GNK : NetworkedBehaviour
{
    // Start is called before the first frame update

    public static InputHandler ih;
    public int defaultServerPort;
    public int defaultWebSocketPort;
    public int defaultDiscoveryPort;

    private float timer;
    void Start()
    {
        DontDestroyOnLoad(this);
        timer = 0.0f;
        //DontDestroyOnLoad(this);
    }

    /**
    void setupNetworking()
    {
        CustomMessagingManager.RegisterNamedMessageHandler("myMessageName", (senderClientId, stream)
        {
                using (PooledBitReader reader = PooledBitReader.Get(stream))
                {
                    StringBuilder stringBuilder = reader.ReadString(); //Example
                    string message = stringBuilder.ToString();
                }
            }
        );

            //Sending
            CustomMessagingManager.SendNamedMessage("myMessageName", clientId, myStream, "myCustomChannel"); //Channel is optional.
        }
    }
    // **/
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GNK");

        if (objs.Length > 1)
        {
            //Destroy(objs[0]);
        }
    }

    void Update()
    {
        if (Physics.autoSimulation)
            return;

        timer += Time.deltaTime;
        while (timer >= Time.fixedDeltaTime)
        {
            timer -= Time.fixedDeltaTime;
            Physics.Simulate(Time.fixedDeltaTime);
        }
    }
}
