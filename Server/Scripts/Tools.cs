using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DevelopersHub.RealtimeNetworking.Server
{
    class Tools
    {

        public static string GenerateToken()
        {
            return Path.GetRandomFileName().Remove(8, 1);
        }

        public static string GetIP(AddressFamily type)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == type)
                {
                    return ip.ToString();
                }
            }
            return "0.0.0.0";
        }

        public static string GetExternalIP()
        {
            try
            {
                var ip = IPAddress.Parse(new WebClient().DownloadString("https://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim());
                return ip.ToString();
            }
            catch (Exception)
            {
                try
                {
                    StreamReader sr = new StreamReader(WebRequest.Create("https://checkip.dyndns.org").GetResponse().GetResponseStream());
                    string[] ipAddress = sr.ReadToEnd().Trim().Split(':')[1].Substring(1).Split('<');
                    return ipAddress[0];
                }
                catch (Exception)
                {
                    return "0.0.0.0";
                }
            }
        }

        public static void LogError(string message, string trace, string folder = "")
        {
            Console.WriteLine("Error:" + "\n" + message + "\n" + trace);
            Task task = Task.Run(() =>
            {
                try
                {
                    string folderPath = Terminal.logFolderPath;
                    if (!string.IsNullOrEmpty(folder))
                    {
                        folderPath = folderPath  + folder + "\\";
                    }
                    string path = folderPath + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss-ffff") + ".txt";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    File.WriteAllText(path, message + "\n" + trace);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:" + "\n" + ex.Message + "\n" + ex.StackTrace);
                }
            });
        }

    }
}