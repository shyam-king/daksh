using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using UnityEngine.Events;
using System;

public class StatusUpdateControl : MonoBehaviour
{
    public string statusCommandPrefix = "STATUS";
    public UnityEvent callback;

    private Socket mSocket;
    private SocketAsyncEventArgs receiveEvent;
    byte[] receiveBuffer;

    public event EventHandler<StatusUpdateEventArgs> StatusUpdated;
    // Start is called before the first frame update
    void Start()
    {
        mSocket = SettingsManager.connectionSocket;
        if (mSocket != null && mSocket.Connected)
        {
            receiveEvent = new SocketAsyncEventArgs();
            receiveEvent.Completed += OnReceiveMessages;
            receiveBuffer = new byte[1024];
            receiveEvent.SetBuffer(receiveBuffer, 0, 1024);

            mSocket.ReceiveAsync(receiveEvent);
        }
    }

    private void OnReceiveMessages(object sender, SocketAsyncEventArgs e)
    {
        string command = Encoding.ASCII.GetString(e.Buffer);
        int len = statusCommandPrefix.Length;

        if (command.StartsWith(statusCommandPrefix + ":"))
        {
            Debug.Log("delegating command");
            if (StatusUpdated != null)
            {
                StatusUpdateEventArgs args = new StatusUpdateEventArgs
                {
                    newStatus = command.Substring(len + 1)
                };
                StatusUpdated(this, args);
            }
            else
            {
                Debug.Log("No members to delegate to");
            }
        }

        Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
        mSocket.ReceiveAsync(receiveEvent);
    }
}

public class StatusUpdateEventArgs : EventArgs
{
    public string newStatus;
}
