namespace DevelopersHub.RealtimeNetworking.Client
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Net;
    using System.Net.Sockets;
    using System;
    using System.Linq;

    public class Client : MonoBehaviour
    {

        private static int dataBufferSize = 4096;
        private static int connectTimeout = 5000;
        private int _id = 0; public int id { get { return _id; } }
        private string _sendToken = "xxxxx"; public string sendToken { get { return _sendToken; } }
        private string _receiveToken = "xxxxx"; public string receiveToken { get { return _receiveToken; } }
        public TCP tcp;
        public UDP udp;
        private bool _isConnected = false; public bool isConnected { get { return _isConnected; } }
        private delegate void PacketHandler(Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers;
        private bool _connecting = false;
        private bool _initialized = false;
        private Settings _settings = null; public Settings settings { get { return _settings; } set { _settings = value; } }

        private static Client _instance = null; public static Client instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<Client>();
                    if (_instance == null)
                    {
                        _instance = new GameObject("Client").AddComponent<Client>();
                    }
                    _instance.Initialize();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            _initialized = true;
            try
            {
                var resources = Resources.LoadAll("", typeof(Settings)).Cast<Settings>();
                foreach (var s in resources)
                {
                    settings = s;
                    break;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            DontDestroyOnLoad(gameObject);
            Threading[] threadings = FindObjectsByType<Threading>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            if (threadings != null && threadings.Length > 0)
            {
                for (int i = 0; i < threadings.Length; i++)
                {
                    Destroy(threadings[i]);
                }
            }
            gameObject.AddComponent<Threading>();
        }

        private void OnApplicationQuit()
        {
            Disconnect(false);
        }

        private void OnDestroy()
        {
            Disconnect(false);
        }

        public void ConnectToServer()
        {
            if (_isConnected || _connecting)
            {
                return;
            }

            _connecting = true;

            tcp = new TCP();
            udp = new UDP();

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

            tcp.Connect();
        }

        public class TCP
        {
            public TcpClient socket;
            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };
                receiveBuffer = new byte[dataBufferSize];
                IAsyncResult result = null;
                bool waiting = false;
                try
                {
                    result = socket.BeginConnect(instance.settings.ip, instance.settings.port, ConnectCallback, socket);
                    waiting = result.AsyncWaitHandle.WaitOne(connectTimeout, false);
                }
                catch (Exception)
                {
                    instance._connecting = false;
                    RealtimeNetworking.instance._Connection(false);
                    return;
                }
                if (!waiting || !socket.Connected)
                {
                    instance._connecting = false;
                    RealtimeNetworking.instance._Connection(false);
                    return;
                }
            }

            private void ConnectCallback(IAsyncResult result)
            {
                socket.EndConnect(result);
                if (!socket.Connected)
                {
                    return;
                }
                instance._connecting = false;
                instance._isConnected = true;
                stream = socket.GetStream();
                receivedData = new Packet();
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
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
                    Debug.Log($"Error sending data to server via TCP: {ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int length = stream.EndRead(result);
                    if (length <= 0)
                    {
                        instance.Disconnect();
                        return;
                    }
                    byte[] data = new byte[length];
                    Array.Copy(receiveBuffer, data, length);
                    receivedData.Reset(CheckData(data));
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch
                {
                    Disconnect();
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
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int id = _packet.ReadInt();
                            packetHandlers[id](_packet);
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

            private void Disconnect()
            {
                instance.Disconnect();
                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;
            }

        }

        public class UDP
        {
            public UdpClient socket;
            public IPEndPoint endPoint;

            public UDP()
            {
                endPoint = new IPEndPoint(IPAddress.Parse(instance.settings.ip), instance.settings.port);
            }

            public void Connect(int port)
            {
                socket = new UdpClient(port);
                socket.Connect(endPoint);
                socket.BeginReceive(ReceiveCallback, null);
                using (Packet _packet = new Packet())
                {
                    SendData(_packet);
                }
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    _packet.InsertInt(instance._id);
                    if (socket != null)
                    {
                        socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"Error sending data to server via UDP: {ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    byte[] data = socket.EndReceive(result, ref endPoint);
                    socket.BeginReceive(ReceiveCallback, null);
                    if (data.Length < 4)
                    {
                        instance.Disconnect();
                        return;
                    }
                    CheckData(data);
                }
                catch
                {
                    Disconnect();
                }
            }

            private void CheckData(byte[] data)
            {
                using (Packet _packet = new Packet(data))
                {
                    int length = _packet.ReadInt();
                    data = _packet.ReadBytes(length);
                }
                Threading.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(data))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });
            }

            private void Disconnect()
            {
                instance.Disconnect();
                endPoint = null;
                socket = null;
            }
        }

        public void Disconnect(bool callEvent = true)
        {
            if (_isConnected)
            {
                _isConnected = false;
                if (tcp.socket != null)
                {
                    tcp.socket.Close();
                }
                if (udp.socket != null)
                {
                    udp.socket.Close();
                }
                if (callEvent)
                {
                    RealtimeNetworking.instance._Disconnected();
                }
            }
        }

        public void ConnectionResponse(bool result, int id, string token1, string token2)
        {
            _id = id;
            _sendToken = token1;
            _receiveToken = token2;
            RealtimeNetworking.instance._Connection(true);
        }

    }
}