using System;
using System.Numerics;

namespace DevelopersHub.RealtimeNetworking.Server
{
    class Sender
    {

        #region Core
        /// <summary>Sends a packet to a client via TCP.</summary>
        /// <param name="clientID">The client to send the packet the packet to.</param>
        /// <param name="packet">The packet to send to the client.</param>
        private static void SendTCPData(int clientID, Packet packet)
        {
            packet.WriteLength();
            Server.clients[clientID].tcp.SendData(packet);
        }

        /// <summary>Sends a packet to a client via UDP.</summary>
        /// <param name="clientID">The client to send the packet the packet to.</param>
        /// <param name="packet">The packet to send to the client.</param>
        private static void SendUDPData(int clientID, Packet packet)
        {
            packet.WriteLength();
            Server.clients[clientID].udp.SendData(packet);
        }

        /// <summary>Sends a packet to all clients via TCP.</summary>
        /// <param name="packet">The packet to send.</param>
        private static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }

        /// <summary>Sends a packet to all clients except one via TCP.</summary>
        /// <param name="exceptClientID">The client to NOT send the data to.</param>
        /// <param name="packet">The packet to send.</param>
        private static void SendTCPDataToAll(int exceptClientID, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != exceptClientID)
                {
                    Server.clients[i].tcp.SendData(packet);
                }
            }
        }

        /// <summary>Sends a packet to all clients via UDP.</summary>
        /// <param name="packet">The packet to send.</param>
        private static void SendUDPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }

        /// <summary>Sends a packet to all clients except one via UDP.</summary>
        /// <param name="exceptClientID">The client to NOT send the data to.</param>
        /// <param name="packet">The packet to send.</param>
        private static void SendUDPDataToAll(int exceptClientID, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != exceptClientID)
                {
                    Server.clients[i].udp.SendData(packet);
                }
            }
        }
        #endregion

        #region TCP
        public static void TCP_Send(int clientID, int packetID)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.NULL))
                {
                    packet.Write(packetID);
                    SendTCPData(clientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.NULL))
                {
                    packet.Write(packetID);
                    SendTCPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.NULL))
                {
                    packet.Write(packetID);
                    SendTCPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, Packet packet)
        {
            try
            {
                if (packet != null)
                {
                    packet.SetID((int)Packet.ID.CUSTOM);
                    SendTCPData(clientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(Packet packet)
        {
            try
            {
                if (packet != null)
                {
                    packet.SetID((int)Packet.ID.CUSTOM);
                    SendTCPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, Packet packet)
        {
            try
            {
                if (packet != null)
                {
                    packet.SetID((int)Packet.ID.CUSTOM);
                    SendTCPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, int packetID, string data)
        {
            try
            {
                if (data != null && clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.STRING))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID, string data)
        {
            try
            {
                if (data != null)
                {
                    using (Packet packet = new Packet((int)Packet.ID.STRING))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPDataToAll(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID, string data)
        {
            try
            {
                if (data != null)
                {
                    using (Packet packet = new Packet((int)Packet.ID.STRING))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPDataToAll(excludedClientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, int packetID, byte data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.BYTE))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID, byte data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.BYTE))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID, byte data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.BYTE))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, int packetID, byte[] data)
        {
            try
            {
                if (data != null && clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.BYTES))
                    {
                        packet.Write(packetID);
                        packet.Write(data.Length);
                        packet.Write(data);
                        SendTCPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID, byte[] data)
        {
            try
            {
                if (data != null)
                {
                    using (Packet packet = new Packet((int)Packet.ID.BYTES))
                    {
                        packet.Write(packetID);
                        packet.Write(data.Length);
                        packet.Write(data);
                        SendTCPDataToAll(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID, byte[] data)
        {
            try
            {
                if (data != null)
                {
                    using (Packet packet = new Packet((int)Packet.ID.BYTES))
                    {
                        packet.Write(packetID);
                        packet.Write(data.Length);
                        packet.Write(data);
                        SendTCPDataToAll(excludedClientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, int packetID, Vector3 data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.VECTOR3))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID, Vector3 data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.VECTOR3))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID, Vector3 data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.VECTOR3))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, int packetID, Quaternion data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.QUATERNION))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID, Quaternion data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.QUATERNION))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID, Quaternion data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.QUATERNION))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, int packetID, int data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.INTEGER))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID, int data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.INTEGER))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID, int data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.INTEGER))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, int packetID, bool data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.BOOLEAN))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID, bool data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.BOOLEAN))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID, bool data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.BOOLEAN))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, int packetID, float data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.FLOAT))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID, float data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.FLOAT))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID, float data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.FLOAT))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, int packetID, long data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.LONG))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID, long data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.LONG))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID, long data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.LONG))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_Send(int clientID, int packetID, short data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.SHORT))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendTCPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAll(int packetID, short data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.SHORT))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void TCP_SentToAllExeptOne(int excludedClientID, int packetID, short data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.SHORT))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendTCPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }
        #endregion

        #region UDP
        public static void UDP_Send(int clientID, int packetID)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.NULL))
                {
                    packet.Write(packetID);
                    SendUDPData(clientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.NULL))
                {
                    packet.Write(packetID);
                    SendUDPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.NULL))
                {
                    packet.Write(packetID);
                    SendUDPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, Packet packet)
        {
            try
            {
                if (packet != null)
                {
                    packet.SetID((int)Packet.ID.CUSTOM);
                    SendUDPData(clientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(Packet packet)
        {
            try
            {
                if (packet != null)
                {
                    packet.SetID((int)Packet.ID.CUSTOM);
                    SendUDPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, Packet packet)
        {
            try
            {
                if (packet != null)
                {
                    packet.SetID((int)Packet.ID.CUSTOM);
                    SendUDPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, int packetID, string data)
        {
            try
            {
                if (data != null && clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.STRING))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID, string data)
        {
            try
            {
                if (data != null)
                {
                    using (Packet packet = new Packet((int)Packet.ID.STRING))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPDataToAll(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID, string data)
        {
            try
            {
                if (data != null)
                {
                    using (Packet packet = new Packet((int)Packet.ID.STRING))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPDataToAll(excludedClientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, int packetID, byte data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.BYTE))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID, byte data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.BYTE))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID, byte data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.BYTE))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, int packetID, byte[] data)
        {
            try
            {
                if (data != null && clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.BYTES))
                    {
                        packet.Write(packetID);
                        packet.Write(data.Length);
                        packet.Write(data);
                        SendUDPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID, byte[] data)
        {
            try
            {
                if (data != null)
                {
                    using (Packet packet = new Packet((int)Packet.ID.BYTES))
                    {
                        packet.Write(packetID);
                        packet.Write(data.Length);
                        packet.Write(data);
                        SendUDPDataToAll(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID, byte[] data)
        {
            try
            {
                if (data != null)
                {
                    using (Packet packet = new Packet((int)Packet.ID.BYTES))
                    {
                        packet.Write(packetID);
                        packet.Write(data.Length);
                        packet.Write(data);
                        SendUDPDataToAll(excludedClientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, int packetID, Vector3 data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.VECTOR3))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID, Vector3 data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.VECTOR3))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID, Vector3 data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.VECTOR3))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, int packetID, Quaternion data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.QUATERNION))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID, Quaternion data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.QUATERNION))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID, Quaternion data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.QUATERNION))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, int packetID, int data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.INTEGER))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID, int data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.INTEGER))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID, int data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.INTEGER))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, int packetID, bool data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.BOOLEAN))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID, bool data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.BOOLEAN))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID, bool data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.BOOLEAN))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, int packetID, float data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.FLOAT))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID, float data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.FLOAT))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID, float data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.FLOAT))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, int packetID, long data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.LONG))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID, long data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.LONG))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID, long data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.LONG))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_Send(int clientID, int packetID, short data)
        {
            try
            {
                if (clientID > 0)
                {
                    using (Packet packet = new Packet((int)Packet.ID.SHORT))
                    {
                        packet.Write(packetID);
                        packet.Write(data);
                        SendUDPData(clientID, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAll(int packetID, short data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.SHORT))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void UDP_SentToAllExeptOne(int excludedClientID, int packetID, short data)
        {
            try
            {
                using (Packet packet = new Packet((int)Packet.ID.SHORT))
                {
                    packet.Write(packetID);
                    packet.Write(data);
                    SendUDPDataToAll(excludedClientID, packet);
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }
        #endregion

    }
}