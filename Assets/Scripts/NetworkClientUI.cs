using System.Collections;
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


    static string serverMessage;

    static short messageNumber = 999;

    static int clientID = 0;
    

    private void OnGUI()
    {
        ipaddress = Network.player.ipAddress;
        GUI.Box(new Rect(10, Screen.height - 50, 100, 50), ipaddress);
        GUI.Label(new Rect(20, Screen.height - 30, 100, 20), "Status: " + client.isConnected);

        GUI.Box(new Rect(150, Screen.height - 50, 100, 50), "Server message");
        GUI.Label(new Rect(150, Screen.height - 30, 300, 20), serverMessage = "" + clientID);
        
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

    void SetClientID(string number)
    {
        if (int.TryParse(number, out clientID)) { }
    }

    static public void SendJoystickInfo(float hDelta, float vDelta)
    {
        if (client.isConnected)
        {
            StringMessage msg = new StringMessage();
            msg.value = 1 + "|" + clientID + "|" + hDelta + "|" + vDelta;
            client.Send(messageNumber, msg);
        }
    }
    static public void SendTiltInfo(float delta0, float delta1, float delta2)
    {
        if (client.isConnected)
        {
            StringMessage msg = new StringMessage();
            msg.value = 3 + "|" + clientID + "|" + delta0 + "|" + delta1 + "|" + delta2;
            client.Send(messageNumber, msg);
        }
    }

    static public void SendButtonInfo( int pressed, int buttonID)
    {
        if (client.isConnected)
        {
            StringMessage msg = new StringMessage();
            msg.value = 2 + "|" + clientID + "|" + pressed + "|" + buttonID;
            client.Send(messageNumber, msg);
        }
    }

    public void GetMessageFromServer(NetworkMessage message)
    {
        StringMessage msg = new StringMessage();
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        if (deltas[0] == "0")
        {
            SetClientID(deltas[1]);
        }
    }

}
