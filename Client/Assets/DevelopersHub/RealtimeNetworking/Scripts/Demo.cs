namespace DevelopersHub.RealtimeNetworking.Client
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Demo : MonoBehaviour
    {

        private void Start()
        { 
            // Creating event listeners
            RealtimeNetworking.OnDisconnectedFromServer += Disconnected;
            RealtimeNetworking.OnConnectingToServerResult += ConnectResult;
            RealtimeNetworking.OnPacketReceived += PacketReceived;

            // Try to connect the server
            RealtimeNetworking.Connect();
        }

        private void Disconnected()
        {
            Debug.Log("Disconnected from server.");
        }

        private void ConnectResult(bool successful)
        {
            if (successful)
            {
                Debug.Log("Connected to server successfully.");

                // Send String Example
                Sender.TCP_Send(123, "Hello world from " + SystemInfo.deviceName);

                // Send Packet Example
                Packet packet = new Packet();
                packet.Write(666);
                packet.Write("Foo Bar");
                packet.Write(3.14f);
                packet.Write(transform.rotation);
                packet.Write(false);
                Sender.TCP_Send(packet);
            }
            else
            {
                Debug.Log("Failed to connect the server.");
            }
        }

        private void PacketReceived(Packet packet)
        {
            int code = packet.ReadInt();
            if(code == 555)
            {
                string time = packet.ReadString();
                Debug.Log("Server Time: " + time);
            }
        }

    }
}