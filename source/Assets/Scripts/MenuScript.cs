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
    public GameObject multiplayerWebGLMenuCanvas;
    public GameObject settingsMenuCanvas;
    public GameObject creditCanvas;
    public TextAsset credits;
    public Text creditField;
    public float creditEnd;
    public float creditSpeed;
    public NetworkManager manager;
    public AudioSource musicPlayer;
    public AudioClip menuSong;
    public AudioClip creditSong;

    float creditTime;


    // Start is called before the first frame update
    void Start()
    {
        ResetMainMenu();
    }

    public void ResetMainMenu()
    {
        play(menuSong);
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        setCanvas(0);
    }

    public void OpenSingleplayerMenu()
    {
        setCanvas(1);
    }

    public void OpenMultiplayerMenu()
    {
        if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
        {
            setCanvas(2);
        }
        else
        {
            setCanvas(3);
        }
    }

    public void StartSingleplayer()
    {
        SceneManager.LoadScene("Singleplayer");
    }

    public void toggleWebSockets(Text button)
    {
        manager.useWebSockets = !manager.useWebSockets;
        if (manager.useWebSockets)
        {
            button.text = "Web Sockets On";
        }
        else
        {
            button.text = "Web Sockets Off";
        }
    }

    public void StartMultiplayerHost()
    {
        manager.networkAddress = "localhost";
        manager.StartHost();
    }

    public void setClientAddress(Text inputField)
    {
        manager.networkAddress = inputField.text.ToString();
    }

    public void StartMultiplayerClient(Text inputField)
    {
        setClientAddress(inputField);
        //Debug.Log(manager.networkAddress);
        manager.StartClient();
    }

    public void StartMultiplayerServer()
    {
        setCanvas(-1);
    }

    public void OpenSettings()
    {
        setCanvas(4);
    }

    public void OpenCredits()
    {
        setCanvas(5);
        creditField.text = credits.text;
        creditTime = Time.time;
        play(creditSong);
    }

    void setCanvas(int index)
    {
        mainMenuCanvas.active = index == 0;
        singleplayerMenuCanvas.active = index == 1;
        multiplayerMenuCanvas.active = index == 2;
        multiplayerWebGLMenuCanvas.active = index == 3;
        settingsMenuCanvas.active = index == 4;
        creditCanvas.active = index == 5;

    }

    public void Update()
    {
        //Debug.Log(creditTime);
        if (creditTime > 0)
        {
            creditField.rectTransform.anchoredPosition = new Vector2(0, (Time.time - creditTime) * 30);
        }
        if (creditTime != 0 && (Time.time - creditTime) > creditEnd)
        {
            ResetMainMenu();
            creditTime = 0;
        }
    }

    private void play(AudioClip clip)
    {
        musicPlayer.Stop();
        musicPlayer.clip = clip;
        musicPlayer.Play();
    }
}
