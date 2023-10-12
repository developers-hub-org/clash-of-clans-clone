using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DevelopersHub.RealtimeNetworking.Server
{
    class Receiver
    {

        public static void Initialization(int clientID, Packet packet)
        {
            string token = packet.ReadString();
            Server.clients[clientID].receiveToken = token;
        }

        public static void ReceiveNull(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                Terminal.ReceivedEvent(clientID, packetID);
            }
        }

        public static void ReceiveCustom(int clientID, Packet packet)
        {
            if (packet != null)
            {
                Terminal.ReceivedPacket(clientID, packet);
            }
        }

        public static void ReceiveString(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                string data = packet.ReadString();
                Terminal.ReceivedString(clientID, packetID, data);
            }
        }

        public static void ReceiveInteger(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                int data = packet.ReadInt();
                Terminal.ReceivedInteger(clientID, packetID, data);
            }
        }

        public static void ReceiveBoolean(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                bool data = packet.ReadBool();
                Terminal.ReceivedBoolean(clientID, packetID, data);
            }
        }

        public static void ReceiveFloat(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                float data = packet.ReadFloat();
                Terminal.ReceivedFloat(clientID, packetID, data);
            }
        }

        public static void ReceiveShort(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                short data = packet.ReadShort();
                Terminal.ReceivedShort(clientID, packetID, data);
            }
        }

        public static void ReceiveLong(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                long data = packet.ReadLong();
                Terminal.ReceivedLong(clientID, packetID, data);
            }
        }

        public static void ReceiveVector3(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                Vector3 data = packet.ReadVector3();
                Terminal.ReceivedVector3(clientID, packetID, data);
            }
        }

        public static void ReceiveQuaternion(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                Quaternion data = packet.ReadQuaternion();
                Terminal.ReceivedQuaternion(clientID, packetID, data);
            }
        }

        public static void ReceiveByte(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                byte data = packet.ReadByte();
                Terminal.ReceivedByte(clientID, packetID, data);
            }
        }

        public static void ReceiveBytes(int clientID, Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                int bytesLenght = packet.ReadInt();
                byte[] data = packet.ReadBytes(bytesLenght);
                Terminal.ReceivedBytes(clientID, packetID, data);
            }
        }

    }
}