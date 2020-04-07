using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Net.Sockets;

public class SocketCommandButtonController : MonoBehaviour
{
    public string sendCommand = "LED_ON";

    private SocketAsyncEventArgs sendEvent;
    private void Start()
    {
        Button mButton = GetComponent<Button>();
        if (mButton != null)
        {
            mButton.onClick.AddListener(Trigger);
        }

        sendEvent = new SocketAsyncEventArgs();
        sendEvent.Completed += SocketSendCompleted;
    }

    private void SocketSendCompleted(object sender, SocketAsyncEventArgs e)
    {
        Debug.Log("sent: " + Encoding.ASCII.GetString(e.Buffer));
    }

    // Start is called before the first frame update
    public void Trigger()
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
}
