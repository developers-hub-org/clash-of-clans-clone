using System;
using System.Net;
using System.Net.Sockets;

namespace DevelopersHub.RealtimeNetworking.Server
{
    class Client
    {

        public static int dataBufferSize = 4096;
        public int id;
        public TCP tcp;
        public UDP udp;
        public string sendToken = "xxxxx";
        public string receiveToken = "xxxxx";

        public string device = "";
        public long account = 0;
        public bool connected = false;

        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
            udp = new UDP(id);
        }

        public class TCP
        {
            public TcpClient socket;
            private readonly int id;
            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public TCP(int _id)
            {
                id = _id;
            }

            public void Initialize(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;
                stream = socket.GetStream();
                receivedData = new Packet();
                receiveBuffer = new byte[dataBufferSize];
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, IncomingData, null);
                using (Packet packet = new Packet((int)Packet.ID.INITIALIZATION))
                {
                    Server.clients[id].sendToken = Tools.GenerateToken();
                    packet.Write(id);
                    packet.Write(Server.clients[id].sendToken);
                    packet.WriteLength();
                    Server.clients[id].tcp.SendData(packet);
                }
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending data to client id {0} via TCP: {1}", id, ex.Message);
                }
            }

            private void IncomingData(IAsyncResult result)
            {
                try
                {
                    int length = stream.EndRead(result);
                    if (length <= 0)
                    {
                        Server.clients[id].Disconnect();
                        return;
                    }
                    byte[] data = new byte[length];
                    Array.Copy(receiveBuffer, data, length);
                    receivedData.Reset(CheckData(data));
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, IncomingData, null);
                }
                catch /*(Exception ex)*/
                {
                    // Tools.LogError(ex.Message, ex.StackTrace, "Client");
                    Server.clients[id].Disconnect();
                }
            }

            private bool CheckData(byte[] _data)
            {
                int length = 0;
                receivedData.SetBytes(_data);
                if (receivedData.UnreadLength() >= 4)
                {
                    length = receivedData.ReadInt();
                    if (length <= 0)
                    {
                        return true;
                    }
                }
                while (length > 0 && length <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(length);
                    Threading.ExecuteOnMainThread(() =>
                    {
                        try
                        {
                            using (Packet _packet = new Packet(_packetBytes))
                            {
                                int _packetId = _packet.ReadInt();
                                Server.packetHandlers[_packetId](id, _packet);
                            }
                        }
                        catch (Exception ex)
                        {
                            Tools.LogError(ex.Message, ex.StackTrace, "Packet");
                        }
                    });
                    length = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        length = receivedData.ReadInt();
                        if (length <= 0)
                        {
                            return true;
                        }
                    }
                }
                if (length <= 1)
                {
                    return true;
                }
                return false;
            }

            public void Disconnect()
            {
                socket.Close();
                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;
            }
        }

        public class UDP
        {
            public IPEndPoint endPoint;
            private int id;

            public UDP(int _id)
            {
                id = _id;
            }

            public void Connect(IPEndPoint _endPoint)
            {
                endPoint = _endPoint;
            }

            public void SendData(Packet _packet)
            {
                Server.SendDataUDP(endPoint, _packet);
            }

            public void CheckData(Packet _packetData)
            {
                int _packetLength = _packetData.ReadInt();
                byte[] _packetBytes = _packetData.ReadBytes(_packetLength);
                Threading.ExecuteOnMainThread(() =>
                {
                    try
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            Server.packetHandlers[_packetId](id, _packet);
                        }
                    }
                    catch (Exception ex)
                    {
                        Tools.LogError(ex.Message, ex.StackTrace);
                    }
                });
            }

            public void Disconnect()
            {
                endPoint = null;
            }
        }

        public void Disconnect()
        {
            if(tcp != null && tcp.socket != null)
            {
                if(tcp.socket.Client != null)
                {
                    IPEndPoint ip = tcp.socket.Client.RemoteEndPoint as IPEndPoint;
                    Terminal.OnClientDisconnected(id, ip.Address.ToString());
                }
                else
                {
                    Terminal.OnClientDisconnected(id, "none");
                }
                tcp.Disconnect();
            }
            else
            {
                Terminal.OnClientDisconnected(id, "none");
            }
            if (udp.endPoint != null)
            {
                udp.Disconnect();
            }
            device = "";
            account = 0;
        }

    }
}