namespace DevelopersHub.RealtimeNetworking.Client
{
    using System.Net;
    using UnityEngine;

    public class Receiver : MonoBehaviour
    {

        public static void Initialization(Packet packet)
        {
            int id = packet.ReadInt();
            string receiveToken = packet.ReadString();
            string sendToken = Tools.GenerateToken();
            Client.instance.ConnectionResponse(true, id, sendToken, receiveToken);
            using (Packet response = new Packet((int)Packet.ID.INITIALIZATION))
            {
                response.Write(receiveToken);
                response.WriteLength();
                Client.instance.tcp.SendData(response);
            }
            Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }

        public static void ReceiveNull(Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                RealtimeNetworking.instance._ReceiveNull(packetID);
            }
        }

        public static void ReceiveCustom(Packet packet)
        {
            if (packet != null)
            {
                RealtimeNetworking.instance._ReceivePacket(packet);
            }
        }

        public static void ReceiveString(Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                string data = packet.ReadString();
                RealtimeNetworking.instance._ReceiveString(packetID, data);
            }
        }

        public static void ReceiveInteger(Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                int data = packet.ReadInt();
                RealtimeNetworking.instance._ReceiveInteger(packetID, data);
            }
        }

        public static void ReceiveBoolean(Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                bool data = packet.ReadBool();
                RealtimeNetworking.instance._ReceiveBoolean(packetID, data);
            }
        }

        public static void ReceiveFloat(Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                float data = packet.ReadFloat();
                RealtimeNetworking.instance._ReceiveFloat(packetID, data);
            }
        }

        public static void ReceiveShort(Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                short data = packet.ReadShort();
                RealtimeNetworking.instance._ReceiveShort(packetID, data);
            }
        }

        public static void ReceiveLong(Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                long data = packet.ReadLong();
                RealtimeNetworking.instance._ReceiveLong(packetID, data);
            }
        }

        public static void ReceiveVector3(Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                System.Numerics.Vector3 data = packet.ReadVector3();
                RealtimeNetworking.instance._ReceiveVector3(packetID, new Vector3(data.X, data.Y, data.Z));
            }
        }

        public static void ReceiveQuaternion(Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                System.Numerics.Quaternion data = packet.ReadQuaternion();
                RealtimeNetworking.instance._ReceiveQuaternion(packetID, new Quaternion(data.X, data.Y, data.Z, data.W));
            }
        }

        public static void ReceiveByte(Packet packet)
        {
            if(packet != null)
            {
                int packetID = packet.ReadInt();
                byte data = packet.ReadByte();
                RealtimeNetworking.instance._ReceiveByte(packetID, data);
            }
        }

        public static void ReceiveBytes(Packet packet)
        {
            if (packet != null)
            {
                int packetID = packet.ReadInt();
                int bytesLenght = packet.ReadInt();
                byte[] data = packet.ReadBytes(bytesLenght);
                RealtimeNetworking.instance._ReceiveBytes(packetID, data);
            }
        }

    }
}