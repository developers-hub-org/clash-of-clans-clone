namespace DevelopersHub.RealtimeNetworking.Client
{
    using UnityEngine;

    public class RealtimeNetworking : MonoBehaviour
    {

        #region Events
        public static event NoCallback OnDisconnectedFromServer;
        public static event ActionCallback OnConnectingToServerResult;
        public static event PacketCallback OnPacketReceived;
        public static event NullCallback OnEmptyReceived;
        public static event StringCallback OnStringReceived;
        public static event IntegerCallback OnIntegerReceived;
        public static event BooleanCallback OnBooleanReceived;
        public static event FloatCallback OnFloatReceived;
        public static event ShortCallback OnShortReceived;
        public static event LongCallback OnLongReceived;
        public static event Vector3Callback OnVector3Received;
        public static event QuaternionCallback OnQuaternionReceived;
        public static event ByteCallback OnByteReceived;
        public static event BytesCallback OnByteArrayReceived;
        public delegate void ActionCallback(bool successful);
        public delegate void NoCallback();
        public delegate void PacketCallback(Packet packet);
        public delegate void NullCallback(int id);
        public delegate void StringCallback(int id, string value);
        public delegate void IntegerCallback(int id, int value);
        public delegate void BooleanCallback(int id, bool value);
        public delegate void FloatCallback(int id, float value);
        public delegate void ShortCallback(int id, short value);
        public delegate void LongCallback(int id, long value);
        public delegate void Vector3Callback(int id, Vector3 value);
        public delegate void QuaternionCallback(int id, Quaternion value);
        public delegate void ByteCallback(int id, byte value);
        public delegate void BytesCallback(int id, byte[] value);
        #endregion

        private bool _initialized = false;

        private static RealtimeNetworking _instance = null; public static RealtimeNetworking instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RealtimeNetworking>();
                    if (_instance == null)
                    {
                        _instance = Client.instance.gameObject.AddComponent<RealtimeNetworking>();
                    }
                    _instance.Initialize();
                }
                return _instance;
            }
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            _initialized = true;
        }

        public static void Connect()
        {
            Client.instance.ConnectToServer();
        }

        public void _Connection(bool result)
        {
            if (OnConnectingToServerResult != null)
            {
                OnConnectingToServerResult.Invoke(result);
            }
        }

        public void _Disconnected()
        {
            if (OnDisconnectedFromServer != null)
            {
                OnDisconnectedFromServer.Invoke();
            }
        }

        public void _ReceivePacket(Packet packet)
        {
            if (OnPacketReceived != null)
            {
                OnPacketReceived.Invoke(packet);
            }
        }

        public void _ReceiveNull(int id)
        {
            if (OnEmptyReceived != null)
            {
                OnEmptyReceived.Invoke(id);
            }
        }

        public void _ReceiveString(int id, string value)
        {
            if (OnStringReceived != null)
            {
                OnStringReceived.Invoke(id, value);
            }
        }

        public void _ReceiveInteger(int id, int value)
        {
            if (OnIntegerReceived != null)
            {
                OnIntegerReceived.Invoke(id, value);
            }
        }

        public void _ReceiveFloat(int id, float value)
        {
            if (OnFloatReceived != null)
            {
                OnFloatReceived.Invoke(id, value);
            }
        }

        public void _ReceiveBoolean(int id, bool value)
        {
            if (OnBooleanReceived != null)
            {
                OnBooleanReceived.Invoke(id, value);
            }
        }

        public void _ReceiveShort(int id, short value)
        {
            if (OnShortReceived != null)
            {
                OnShortReceived.Invoke(id, value);
            }
        }

        public void _ReceiveLong(int id, long value)
        {
            if (OnLongReceived != null)
            {
                OnLongReceived.Invoke(id, value);
            }
        }

        public void _ReceiveVector3(int id, Vector3 value)
        {
            if (OnVector3Received != null)
            {
                OnVector3Received.Invoke(id, value);
            }
        }

        public void _ReceiveQuaternion(int id, Quaternion value)
        {
            if (OnQuaternionReceived != null)
            {
                OnQuaternionReceived.Invoke(id, value);
            }
        }

        public void _ReceiveByte(int id, byte value)
        {
            if (OnByteReceived != null)
            {
                OnByteReceived.Invoke(id, value);
            }
        }

        public void _ReceiveBytes(int id, byte[] value)
        {
            if (OnByteArrayReceived != null)
            {
                OnByteArrayReceived.Invoke(id, value);
            }
        }

    }
}