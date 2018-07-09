using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityStandardAssets.CrossPlatformInput;



public class NetworkServerUI : MonoBehaviour
{

    public GameObject player1;
  
    int playercount = 1;

    public string newPort;

    private void OnGUI()
    {
        string ipaddress = Network.player.ipAddress;

        GUI.Box(new Rect(10, Screen.height - 50, 100, 50), ipaddress);
        GUI.Label(new Rect(20, Screen.height - 35, 100, 20), "Status: " + NetworkServer.active);
        GUI.Label(new Rect(20, Screen.height - 20, 100, 20), "Connected: " + NetworkServer.connections.Count);

        newPort = GUI.TextField(new Rect(Screen.width - 110, 10, 100, 20), newPort, 25);

        if (GUI.Button(new Rect(Screen.width - 110, 30, 100, 60), "Update Port"))
        {
            int portNumber;
            if (int.TryParse(newPort, out portNumber))
                NetworkServer.Listen(portNumber);
        }
    }

    void Start ()
    {
        NetworkServer.Listen(2310);
        NetworkServer.RegisterHandler(999, RecieveMessage);
    }

    // GET MESSAGE FROM CLIENT WITH FIRST ID
    private void RecieveMessage(NetworkMessage message)
    {
        
        int messageID;
        bool isPressed = false;
        int buttonID;

        StringMessage msg = new StringMessage();
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        
        
        if (int.TryParse(deltas[0], out messageID))

        if (messageID == 1)
        {
                //Send axis information to player script
                print("Axis information");
        }
        else if (messageID == 2)
        {
             
                int theBool;
            if (int.TryParse(deltas[2], out theBool))

            if (theBool == 1)
            {
                isPressed = true;
            }
            else
            {
                isPressed = false;
            }

            if (int.TryParse(deltas[3], out buttonID))

            switch (buttonID)
            {
                case 1:
                     print("Button 1 pressed: " + isPressed);
                    //send button information (bool) to player
                    break;
                case 2:
                    print("Button 2 pressed: " + isPressed);
                    //send button information (bool) to player
                    break;
                case 3:
                    print("Button 2 pressed: " + isPressed);
                    //send button information (bool) to player
                    break;

                default:
                    break;
            }
        }
    }

    /*WIP 
    * Get player connections and get player id
    * Handle disconnection and reconnection
    */

    // SEND MESSAGE TO SPESIFIC CLIENT
    public void SendMessage(int playerID, int msgID, int msgInfo)
    {
        StringMessage msg = new StringMessage();
        msg.value = msgID + "|" + msgInfo;
        print(msgID + " " + msgInfo);
        NetworkServer.SendToClient(NetworkServer.connections[playerID].connectionId, 999, msg);
    }

    // SEND TO ALL CLIENTS
    void SendToAllClients()
    {
        StringMessage msg = new StringMessage();
        msg.value = 1 + "";
        NetworkServer.SendToAll(999, msg);
    }

}
