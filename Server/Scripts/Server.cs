using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace DevelopersHub.RealtimeNetworking.Server
{
    class Server
    {

        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int clientID, Packet packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static void Start(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;
            Port = port;
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)Packet.ID.STRING, Receiver.ReceiveString },
                { (int)Packet.ID.BOOLEAN, Receiver.ReceiveBoolean },
                { (int)Packet.ID.VECTOR3, Receiver.ReceiveVector3 },
                { (int)Packet.ID.QUATERNION, Receiver.ReceiveQuaternion },
                { (int)Packet.ID.FLOAT, Receiver.ReceiveFloat },
                { (int)Packet.ID.INTEGER, Receiver.ReceiveInteger },
                { (int)Packet.ID.LONG, Receiver.ReceiveLong },
                { (int)Packet.ID.SHORT, Receiver.ReceiveShort },
                { (int)Packet.ID.BYTES, Receiver.ReceiveBytes },
                { (int)Packet.ID.BYTE, Receiver.ReceiveByte },
                { (int)Packet.ID.INITIALIZATION, Receiver.Initialization },
                { (int)Packet.ID.NULL, Receiver.ReceiveNull },
                { (int)Packet.ID.CUSTOM, Receiver.ReceiveCustom },
            };

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(OnConnectedTCP, null);
            udpListener = new UdpClient(Port);
            udpListener.BeginReceive(OnConnectedUDP, null);
            Console.WriteLine("Server started on port {0}", Port);
            Terminal.OnServerStarted();
        }

        private static void OnConnectedTCP(IAsyncResult result)
        {
            try
            {
                TcpClient client = tcpListener.EndAcceptTcpClient(result);
                tcpListener.BeginAcceptTcpClient(OnConnectedTCP, null);
                for (int i = 1; i <= MaxPlayers; i++)
                {
                    if (clients[i].tcp.socket == null)
                    {
                        clients[i].tcp.Initialize(client);
                        IPEndPoint ip = client.Client.RemoteEndPoint as IPEndPoint;
                        Terminal.OnClientConnected(i, ip.Address.ToString());
                        return;
                    }
                }
                Console.WriteLine("{0} failed to connect. Server is at full capacity.", client.Client.RemoteEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void OnConnectedUDP(IAsyncResult result)
        {
            try
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpListener.EndReceive(result, ref clientEndPoint);
                udpListener.BeginReceive(OnConnectedUDP, null);
                if (data.Length < 4)
                {
                    return;
                }
                using (Packet packet = new Packet(data))
                {
                    int id = packet.ReadInt();
                    if (id == 0)
                    {
                        return;
                    }
                    if (clients[id].udp.endPoint == null)
                    {
                        clients[id].udp.Connect(clientEndPoint);
                        return;
                    }
                    if (clients[id].udp.endPoint.ToString() == clientEndPoint.ToString())
                    {
                        clients[id].udp.CheckData(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error receiving UDP data: {0}", ex.Message);
            }
        }

        public static void SendDataUDP(IPEndPoint clientEndPoint, Packet packet)
        {
            try
            {
                if (clientEndPoint != null)
                {
                    udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending data to {0} via UDP: {1}", clientEndPoint, ex.Message);
            }
        }

    }
}