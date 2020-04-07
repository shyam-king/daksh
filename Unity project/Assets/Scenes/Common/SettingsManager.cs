using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public static class SettingsManager 
{
    public static string ipAddress;
    public static int port;
    public static Socket connectionSocket;
}