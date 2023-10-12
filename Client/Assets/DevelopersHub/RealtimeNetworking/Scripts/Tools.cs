namespace DevelopersHub.RealtimeNetworking.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml.Serialization;
    using UnityEngine;

    public static class Tools
    {

        public static string GenerateToken()
        {
            return Path.GetRandomFileName().Remove(8, 1);
        }

    }
}