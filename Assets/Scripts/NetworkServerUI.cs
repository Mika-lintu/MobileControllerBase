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
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    //public GameObject player5;
    //public GameObject player6;
  
    int playercount = 0;

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

    private void Update()
    {
        if (playercount < (NetworkServer.connections.Count - 1))
        {
            playercount = (NetworkServer.connections.Count - 1);
            switch (playercount)
            {
                case 1:
                    SendMessage(1, 0, 1);
                    break;
                case 2:
                    SendMessage(2, 0, 2);
                    break;
                case 3:
                    SendMessage(3, 0, 3);
                    break;
                case 4:
                    SendMessage(4, 0, 4);
                    break;
                //case 5:
                //    SendMessage(5, 0, 5);
                //    break;
                //case 6:
                //    SendMessage(6, 0, 6);
                //    break;
                default:
                    break;
            }
        }
    }

    // DELTAS[0] = Message ID ----------   DELTAS[1] = Player ID
    private void RecieveMessage(NetworkMessage message)
    {
        StringMessage msg = new StringMessage();
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');

        int messageID;
        int playerIDNum;

        if (int.TryParse(deltas[0], out messageID)) { }
        if (int.TryParse(deltas[1], out playerIDNum)) { }

        if (messageID == 1) //Joystick Input message
        {
            JoystickInput(playerIDNum, deltas[2], deltas[3]);
        }
        else if (messageID == 2) // Button input message
        {
            ButtonInput(playerIDNum, deltas[2], deltas[3]);
        }
        else if (messageID == 3) //Tilt input message
        { 
            TiltInput(playerIDNum, deltas[2], deltas[3], deltas[4]);
        }
    }

    void JoystickInput(int playerID, string horizontal, string vertical)
    {
        PlayerInput player = GetPlayerScript(playerID);
        player.axisH = (Convert.ToSingle(horizontal));
        player.axisV = (Convert.ToSingle(vertical));
        print("Axis information H: " + horizontal + " and V: " + vertical);
    }

    void ButtonInput(int playerID, string pressed, string buttonPressed)
    {
        PlayerInput player = GetPlayerScript(playerID);
        int theBool;
        int buttonID;
        if (int.TryParse(pressed, out theBool))
        if (int.TryParse(buttonPressed, out buttonID))

            switch (buttonID)
            {
                case 1:
                    if (theBool == 1)
                    {
                        print("Button 1 pressed");
                        player.Button1(true);
                    }
                    else
                    {
                        print("Button 1 released");
                        player.Button1(false);
                    }
                    
                    break;
                case 2:
                    if (theBool == 1)
                    {
                        print("Button 2 pressed");
                        player.Button2(true);
                    }
                    else
                    {
                        print("Button 2 released");
                        player.Button2(false);
                    }
                    
                    break;
                case 3:
                    if (theBool == 1)
                    {
                        print("Button 3 pressed");
                        player.Button3(true);
                    }
                    else
                    {
                        print("Button 3 released");
                        player.Button3(false);
                    }
                    
                    break;

                default:
                    break;
            }
    }

    void TiltInput(int playerID, string xInput, string yInput, string zInput)
    {
        PlayerInput player = GetPlayerScript(playerID);
        
        if (player != null)
        {
            player.accleX = (Convert.ToSingle(xInput));
            player.accleY = (Convert.ToSingle(yInput));
            player.accleZ = (Convert.ToSingle(zInput));
            print(xInput + " - " + yInput + " - " + zInput);
        }
       
    }

    PlayerInput GetPlayerScript(int id)
    {
        switch (id)
        {
            case 1:
                return player1.GetComponent<PlayerInput>();
            case 2:
                return player2.GetComponent<PlayerInput>();
            case 3:
                return player3.GetComponent<PlayerInput>();
            case 4:
                return player4.GetComponent<PlayerInput>();
            //case 5:
            //    return player5.GetComponent<PlayerInput>();
            //case 6:
            //    return player6.GetComponent<PlayerInput>();
            default:
                break;
        }
        return null;
    }

    /*
    private void RecieveMessage(NetworkMessage message)
    {
        int messageID;
        bool isPressed = false;
        int buttonID;

        StringMessage msg = new StringMessage();
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        

        if (int.TryParse(deltas[0], out messageID))

            if (messageID == 1) //Joystick Input message
            {
                //Send axis information to player script
                player1.GetComponent<PlayerInput>().axisH = (Convert.ToSingle(deltas[1]));;
                player1.GetComponent<PlayerInput>().axisV = (Convert.ToSingle(deltas[2])); ;
                print("Axis information H: " + deltas[1] + " and V: " + deltas[2]);
            }
            else if (messageID == 2) // Button input message
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
           else if (messageID == 3) //Tilt input message
           {
                print("x: " + deltas[1] + "y: " + deltas[2] + "z: " + deltas[3]);
           }
    }
    */

    /*WIP 
    * Get player connections and get player id
    * Handle disconnection and reconnection
    */

    // SEND MESSAGE TO SPESIFIC CLIENT
    public void SendMessage(int playerID, int msgID, int msgInfo)
    {
        StringMessage msg = new StringMessage();
        msg.value = msgID + "|" + msgInfo;
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
