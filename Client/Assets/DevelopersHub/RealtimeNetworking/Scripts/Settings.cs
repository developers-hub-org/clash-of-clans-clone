namespace DevelopersHub.RealtimeNetworking.Client
{
    using System.IO;
    using UnityEngine;

    public class Settings : ScriptableObject
    {

        [Header("Credentials")]
        [Tooltip("Server IP address.")]
        [SerializeField] private string _ip = "127.0.0.1"; public string ip { get { return _ip; } }

        [Tooltip("Server port number.")]
        [SerializeField] private int _port = 5555; public int port { get { return _port; } }

        #if UNITY_EDITOR
        [UnityEditor.MenuItem("Developers Hub/Realtime Networking/Settings")]
        public static void CreateSettings()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(DevelopersHub.RealtimeNetworking.Client.Settings).Name);
            if (guids.Length > 0)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                UnityEditor.EditorUtility.FocusProjectWindow();
                Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(path);
                UnityEditor.Selection.activeObject = obj;
            }
            else
            {
                string path = Application.dataPath + "/DevelopersHub/RealtimeNetworking/Resources";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                DevelopersHub.RealtimeNetworking.Client.Settings asset = ScriptableObject.CreateInstance<DevelopersHub.RealtimeNetworking.Client.Settings>();
                UnityEditor.AssetDatabase.CreateAsset(asset, "Assets/DevelopersHub/RealtimeNetworking/Resources/Settings.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.EditorUtility.FocusProjectWindow();
                UnityEditor.Selection.activeObject = asset;
            }
        }
        #endif

    }
}