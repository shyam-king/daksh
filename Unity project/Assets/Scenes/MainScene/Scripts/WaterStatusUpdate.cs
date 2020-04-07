using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Text;
using System.Net;

public class WaterStatusUpdate : MonoBehaviour
{
    public RawImage waterLevelIndicator;
    public RawImage waterLevelIndicatorBackground;
    bool activate = false;
    bool activated = false;
    float waterLevelHeight = 5;
    public float maxHeight = 5;
    public Button showButton;
    Color waterLevelColor = Color.green;
    int debugCase = 0;
    bool showDebug = false;
    public Text debugText;
    public Text pressureText;
    bool reset = false;
    bool normal = false;
    int fakeLevel = 0;

    public GameObject debugButton;
    bool debugMode = false;

    public Button normalButton;

    public Button showDebugTwoButton;

    private void Start()
    {
        StatusUpdateControl control = GetComponent<StatusUpdateControl>();
        control.StatusUpdated += OnUpdate;

        if (waterLevelIndicator != null)
        {
            waterLevelIndicator.gameObject.SetActive(false);
        }

        if (waterLevelIndicatorBackground != null)
        {
            waterLevelIndicatorBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(3, maxHeight);
        }

        if (showButton != null)
        {
            showButton.onClick.AddListener(OnShowButtonClick);
        }

        if (showDebugTwoButton != null)
            showDebugTwoButton.onClick.AddListener(OnShow2Cick);

        normalButton.onClick.AddListener(OnNormalClick);

        StartCoroutine(SendStatusPrompt());
        
    }

    void OnNormalClick()
    {
        activated = true;
        pressureText.text = "Pressure: 1atm";
        waterLevelHeight = 4 / 10.0f * maxHeight;
        normal = true;
        debugMode = false;
    }

    void OnShowButtonClick()
    {
        activated = true;
        debugText.text = "Level is high, pressure is normal, problem is probably with input valve.";
        pressureText.text = "Pressure: 1atm";
        reset = true;
        waterLevelHeight = 0;
        normal = false;
    }

    void OnShow2Cick()
    {
        activated = true;
        reset = true;
        debugText.text = "Level and Pressure are high, problem is probably with ouput valve.";
        pressureText.text = "Pressure: 5atm";
        waterLevelHeight = 0;
        normal = false;
    }
    

    IEnumerator SendStatusPrompt()
    {
        SocketAsyncEventArgs sendEvent = new SocketAsyncEventArgs();
        sendEvent.Completed += SocketSendCompleted;
        string sendCommand = "LEVEL_CHECK";
        while (true)
        {
            if (activated)
            {
                if (SettingsManager.connectionSocket != null && SettingsManager.connectionSocket.Connected)
                {
                    if (reset)
                    {
                        sendCommand = "LEVEL_RESET";
                        reset = false;
                    }
                    else sendCommand = "LEVEL_CHECK";
                    Debug.Log("Sending " + sendCommand);
                
                    sendEvent.SetBuffer(Encoding.ASCII.GetBytes(sendCommand), 0, Encoding.ASCII.GetByteCount(sendCommand));

                    SettingsManager.connectionSocket.SendAsync(sendEvent);
                }
                else
                {
                    Debug.Log("No valid socket connection!");
                }
            }
            yield return new WaitForSecondsRealtime(0.75f);
        }
    }

    private void SocketSendCompleted(object sender, SocketAsyncEventArgs e)
    {
        Debug.Log("sent: " + Encoding.ASCII.GetString(e.Buffer));
    }

    private void Update()
    {
        waterLevelIndicator.gameObject.SetActive(activate);
        if (normal)
            waterLevelHeight = 4 / 10.0f * maxHeight;
        waterLevelIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2(3, waterLevelHeight);
        if (waterLevelHeight <= 5 / 10.0f * maxHeight)
            waterLevelColor = Color.green;
        waterLevelIndicator.color = waterLevelColor;
        if (debugButton != null)
        {
            debugButton.gameObject.SetActive(debugMode);
        }
        
    }

    void OnUpdate(object sender, StatusUpdateEventArgs e)
    {
        int level = int.Parse(e.newStatus);
        if (level > 10) level = 10;
        if (level < 0) level = 0;

        Debug.Log(level);
        if (waterLevelIndicator != null)
        {
            Debug.Log("Should be visible now!");
            activate = true;
            waterLevelHeight = level / 10.0f * maxHeight;
            if (level < 6)
            {
                debugMode = false;
                waterLevelColor = Color.green;
            }
            else
            {
                debugMode = true;
                waterLevelColor = Color.red;
            }
        }
    }

    IEnumerator FakeCaseOne()
    {
        activated = false;
        for (int i = 4; i < 10; i++)
        {
            waterLevelHeight = i / 10.0f * maxHeight;
            if (i < 6)
            {
                debugMode = false;
                waterLevelColor = Color.green;
            }
            else
            {
                debugMode = true;
                waterLevelColor = Color.red;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
