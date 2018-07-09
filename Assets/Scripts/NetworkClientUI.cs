﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


public class NetworkClientUI : MonoBehaviour
{

    public string serverIP;
    public string portNumber;
    static string ipaddress;
    
    static NetworkClient client;


    string serverMessage;

    static short messageNumber = 999;
    

    private void OnGUI()
    {
        ipaddress = Network.player.ipAddress;
        GUI.Box(new Rect(10, Screen.height - 50, 100, 50), ipaddress);
        GUI.Label(new Rect(20, Screen.height - 30, 100, 20), "Status: " + client.isConnected);

        GUI.Box(new Rect(150, Screen.height - 50, 100, 50), "Server message");
        GUI.Label(new Rect(150, Screen.height - 30, 300, 20), serverMessage);
        
        if (!client.isConnected)
        {
            serverIP = GUI.TextField(new Rect(Screen.width - 110, 10, 100, 20),serverIP,25);
            portNumber = GUI.TextField(new Rect(Screen.width - 110, 50, 100, 20), portNumber, 25);
            if (GUI.Button(new Rect(10, 10, 60, 50), "Connect"))
            {
                Connect();                
            }
        }
    }

    void Start ()
    {
        client = new NetworkClient();
        client.RegisterHandler(999, GetMessageFromServer);
    }

   
    void Connect()
    {
        int port;
        if (int.TryParse(portNumber, out port))
        client.Connect(serverIP, port);
    }

    static public void FirstMessage()
    {
        if (client.isConnected)
        {
            StringMessage msg = new StringMessage();
            msg.value = ipaddress;
            client.Send(999, msg);
        }
    }

    static public void SendJoystickInfo(float hDelta, float vDelta)
    {
        if (client.isConnected)
        {
            StringMessage msg = new StringMessage();
            msg.value = 1 + "|" + hDelta + "|" + vDelta;
            client.Send(messageNumber, msg);
        }
    }

    static public void SendButtonInfo(string name, int pressed, int buttonID)
    {
        if (client.isConnected)
        {
            StringMessage msg = new StringMessage();
            msg.value = 2 + "|" + name + "|" + pressed + "|" + buttonID;
            client.Send(messageNumber, msg);
        }
    }

    public void GetMessageFromServer(NetworkMessage message)
    {
        StringMessage msg = new StringMessage();
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        serverMessage = deltas[0];
    }

}