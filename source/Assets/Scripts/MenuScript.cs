using MLAPI;
using MLAPI.Transports.UNET;
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
    public Text qualityText;
    public Text inputSchemeText;
    public double creditEnd;
    public double creditSpeed;
    public NetworkManager manager;
    public AudioSource musicPlayer;
    public AudioClip menuSong;
    public AudioClip creditSong;

    public bool dontDestroyOnLoad;

    public static double creditTime;
    public static int inputSchemeIndex;
    private static MenuScript instanceRef;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        ResetMainMenu();
#if (UNITY_IOS || UNITY_ANDROID)
        inputSchemeIndex = PlayerPrefs.GetInt("inputSchemeIndex", 2);
#else
        inputSchemeIndex = PlayerPrefs.GetInt("inputSchemeIndex", 1);
#endif
    }

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Menu");

        if (objs.Length > 1)
        {
            Destroy(objs[0]);
        }
    }

    public void ResetMainMenu()
    {
        play(menuSong);
        OpenMainMenu();
        creditTime = 0;
    }

    public void OpenMainMenu()
    {
        setCanvas(0);
        PlayerPrefs.Save();
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

    public void ShowQuality(int level)
    {
        if(level < 0)
        {
            qualityText.text = "Custom";
        }
        else
        {
            qualityText.text = QualitySettings.names[level];
        }
    }

    public void IncrementQuality()
    {
        QualitySettings.IncreaseLevel();
        ShowQuality(QualitySettings.GetQualityLevel());
    }

    public void DecrementQuality()
    {
        QualitySettings.DecreaseLevel();
        ShowQuality(QualitySettings.GetQualityLevel());
    }

    public void ShowInputScheme(InputMode mode)
    {
        if (mode == null)
        {
            inputSchemeText.text = "Custom";
        }
        else
        {
            inputSchemeText.text = mode.ToString();
        }
    }
    public void IncrementInputScheme()
    {
        inputSchemeIndex++;
        PlayerPrefs.SetInt("inputSchemeIndex", inputSchemeIndex);
        ShowInputScheme((InputMode)inputSchemeIndex);
    }

    public void DecrementInputScheme()
    {
        inputSchemeIndex--;
        PlayerPrefs.SetInt("inputSchemeIndex", inputSchemeIndex);
        ShowInputScheme((InputMode)inputSchemeIndex);
    }

    public void StartMultiplayerHost()
    {
        //manager.networkAddress = "localhost";
        NetworkingManager.Singleton.StartHost();
        MLAPI.Connection.NetworkedClient nc = new MLAPI.Connection.NetworkedClient();
        Debug.Log(nc.ClientId);
    }


    public void setClientAddress(Text inputField)
    {
        //manager.networkAddress = inputField.text.ToString();
        NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectAddress = inputField.text.ToString();
        MLAPI.Connection.NetworkedClient nc = new MLAPI.Connection.NetworkedClient();
        Debug.Log(nc.ClientId);
    }

    public void StartMultiplayerClient(Text inputField)
    {
        setClientAddress(inputField);
        //Debug.Log(manager.networkAddress);
        //manager.StartClient();
        NetworkingManager.Singleton.StartClient();
    }

    public void StartMultiplayerServer()
    {
        setCanvas(-1);
    }

    public void OpenSettings()
    {
        ShowQuality(QualitySettings.GetQualityLevel());
        ShowInputScheme((InputMode)inputSchemeIndex);
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
        mainMenuCanvas.SetActive(index == 0);
        singleplayerMenuCanvas.SetActive(index == 1);
        multiplayerMenuCanvas.SetActive(index == 2);
        multiplayerWebGLMenuCanvas.SetActive(index == 3);
        settingsMenuCanvas.SetActive(index == 4);
        creditCanvas.SetActive(index == 5);

    }

    public void Update()
    {
        //Debug.Log(creditTime);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.Quit();
        }
        if (creditTime > 0)
        {
            creditField.rectTransform.anchoredPosition = new Vector2(0, (float)(Time.time - creditTime) * 30);
        }
        if (creditTime != 0 && (Time.time - creditTime) > creditEnd)
        {
            ResetMainMenu();
        }
    }

    private void play(AudioClip clip)
    {
        musicPlayer.Stop();
        musicPlayer.clip = clip;
        musicPlayer.Play();
    }
}
