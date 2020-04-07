using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

public class StreamFrames : MonoBehaviour
{
    bool sending = false;
    public Camera targetCamera;
    public Material material;

    void Start()
    {
        if (targetCamera == null)
            Debug.LogError("Set targetCamera");
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        SendTexture2D(GetTexture2D(source));
            Graphics.Blit(source, destination, material);
    }

    Texture2D GetTexture2D(RenderTexture src)
    {
        Texture2D texture2D = new Texture2D(src.width, src.height, TextureFormat.RGBA32, false);
        
        texture2D.ReadPixels(new Rect(0, 0, src.width, src.height), 0, 0);
        texture2D.Apply();

        return texture2D;
    }

    void SendTexture2D(Texture2D tex)
    {
        Socket mSocket = SettingsManager.connectionSocket;
        if (mSocket != null && mSocket.Connected)
        {
            byte[] PNG = tex.EncodeToJPG(50);
            Debug.Log("frame data length: " + PNG.Length);
            SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
            sendArgs.SetBuffer(PNG, 0, PNG.Length);
            sendArgs.Completed += SendFrameComplete;

            sending = true;
            mSocket.SendAsync(sendArgs);
        }
        
    }

    private void SendFrameComplete(object sender, SocketAsyncEventArgs e)
    {
        Debug.Log("Sent a frame");
        sending = false;
    }
}
