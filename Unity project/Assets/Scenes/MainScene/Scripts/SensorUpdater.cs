using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net.Sockets;

public class SensorUpdater : MonoBehaviour
{
    bool activated = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendStatusPrompt());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SendStatusPrompt()
    {
        SocketAsyncEventArgs sendEvent = new SocketAsyncEventArgs();
        sendEvent.Completed += SocketSendCompleted;
        string sendCommand = "TEMP_PRESS";
        while (true)
        {
            if (activated)
            {
if (SettingsManager.connectionSocket != null && SettingsManager.connectionSocket.Connected)
            {
                Debug.Log("Sending " + sendCommand);
                sendEvent.SetBuffer(Encoding.ASCII.GetBytes(sendCommand), 0, Encoding.ASCII.GetByteCount(sendCommand));

                SettingsManager.connectionSocket.SendAsync(sendEvent);
            }
            else
            {
                Debug.Log("No valid socket connection!");
            }
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void SocketSendCompleted(object sender, SocketAsyncEventArgs e)
    {
        Debug.Log("sent: " + Encoding.ASCII.GetString(e.Buffer));
    }
}
