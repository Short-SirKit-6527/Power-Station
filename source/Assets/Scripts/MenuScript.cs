using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    public GameObject mainMenuCanvas;
    public GameObject singleplayerMenuCanvas;
    public GameObject multiplayerMenuCanvas;
    public NetworkManager manager;

    // Start is called before the first frame update
    void Start()
    {
        OpenMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMainMenu()
    {
        singleplayerMenuCanvas.active = false;
        multiplayerMenuCanvas.active = false;
        mainMenuCanvas.active = true;
    }

    public void OpenSingleplayerMenu()
    {
        mainMenuCanvas.active = false;
        multiplayerMenuCanvas.active = false;
        singleplayerMenuCanvas.active = true;
    }

    public void OpenMultiplayerMenu()
    {
        mainMenuCanvas.active = false;
        singleplayerMenuCanvas.active = false;
        multiplayerMenuCanvas.active = true;
    }

    public void StartSingleplayer()
    {
        SceneManager.LoadScene("Singleplayer");
    }

    public void StartMultiplayerHost()
    {
        manager.networkAddress = "localhost";
        manager.StartHost();
    }

    public void setClientAddress(Text inputField)
    {
        //manager.networkAddress = inputField.text.ToString();
    }

    public void StartMultiplayerClient(Text inputField)
    {
        //Debug.Log(manager.networkAddress);
        manager.StartClient();
    }

    public void StartMultiplayerServer()
    {

    }
}
