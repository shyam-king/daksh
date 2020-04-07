using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class ConnectControl : MonoBehaviour
{
    public Button connectButton;
    public Text ipAddressText;
    public Text portText;
    public Text errorText;
    public Button remoteGuidanceButton;
    // Start is called before the first frame update
    void Start()
    {
        if (connectButton != null)
        {
            connectButton.onClick.AddListener(ConnectButtonOnClick);
        }

        if (connectButton == null)
        {
            Debug.LogWarning("connectButton of EventSystem->connectButtonBehaviour is null");
        }

        if (ipAddressText == null)
        {
            Debug.LogWarning("ipAddressText of EventSystem->connectButtonBehaviour is null");
        }

        if (portText == null)
        {
            Debug.LogWarning("portText of EventSystem->connectButtonBehaviour is null");
        }

        if (errorText == null)
        {
            Debug.LogWarning("errorText of EventSystem->connectButtonBehaviour is null");
        }

        if (remoteGuidanceButton != null)
        {
            remoteGuidanceButton.onClick.AddListener(RemoteGuidanceButtonOnClick);
        }
    }

    private void OnApplicationQuit()
    {
        if (SettingsManager.connectionSocket != null && SettingsManager.connectionSocket.Connected)
        {
            Debug.Log("Closing sockets");
            SettingsManager.connectionSocket.Shutdown(SocketShutdown.Both);
            SettingsManager.connectionSocket.Close();
        }
    }

    void RemoteGuidanceButtonOnClick()
    {
        if (ConnectSocket())
            LoadNextScene(2);
    }

    void ConnectButtonOnClick()
    {
        if (ConnectSocket())
            LoadNextScene();
    }

    bool ConnectSocket()
    {
        bool connected = false;
        if (ipAddressText != null && portText != null)
        {
            Debug.Log(ipAddressText.text);
            Debug.Log(portText.text);

            SettingsManager.ipAddress = ipAddressText.text;
            SettingsManager.port = int.Parse(portText.text);

            Debug.Log("Trying to connect to: " + SettingsManager.ipAddress);
            Debug.Log("At port: " + SettingsManager.port);
            IPAddress ipAddress = IPAddress.Parse(SettingsManager.ipAddress);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, SettingsManager.port);
            SettingsManager.connectionSocket = new Socket(ipAddress.AddressFamily,
                                                            SocketType.Stream,
                                                            ProtocolType.Tcp);


            try
            {
                SettingsManager.connectionSocket.Connect(endPoint);
                connected = true;
            }
            catch (ArgumentNullException ane)
            {
                Debug.LogError("ArgumentNullException: " + ane.ToString());
            }

            catch (SocketException se)
            {
                Debug.LogError("SocketException : " + se.ToString());
            }
            catch (Exception e)
            {
                Debug.LogError("Unexpected exception :" + e.ToString());
            }
            finally
            {
                if (!connected)
                {
                    errorText.text = "Could not connect to the server! Please check the input.";
                }
            }
        }
        return connected;
    }

    void LoadNextScene(int i = 1)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + i);
    }
}
