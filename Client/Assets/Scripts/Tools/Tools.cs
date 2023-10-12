namespace DevelopersHub.ClashOfWhatecer
{
    using System;
    using UnityEngine;

    public class Tools : MonoBehaviour
    {

        public static Resolution GetCurrentResolutionEditor()
        {
            #if UNITY_EDITOR
            if(Application.isPlaying)
            {
                return new Resolution
                {
                    width = Screen.width,
                    height = Screen.height,
                };
            }
            else
            {
                return new Resolution
                {
                    width = Camera.main.pixelWidth,
                    height = Camera.main.pixelHeight,
                };
            }
            #else
            return new Resolution
            {
                width = Screen.width,
                height = Screen.height,
            };
            #endif
        }

        public static Color GenerateRandomColor()
        {
            return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        }

        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", "").Replace("#", "");
            byte a = 255;
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return (Color)(new Color32(r, g, b, a));
        }

        public static string ColorToHex(Color color)
        {
            Color32 color32 = (Color32)color;
            return color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2");
        }

        public static string SecondsToTimeFormat(TimeSpan time)
        {
            string result = "";
            int days = Mathf.FloorToInt((float)time.TotalDays);
            int hours = Mathf.FloorToInt((float)time.TotalHours);
            int mins = Mathf.FloorToInt((float)time.TotalMinutes);
            if (days > 0)
            {
                hours -= (days * 24);
                if (hours > 0)
                {
                    result = days.ToString() + "d " + hours.ToString() + "H";
                }
                else
                {
                    result = days.ToString() + "d";
                }
            }
            else if (hours > 0)
            {
                mins -= (hours * 60);
                if (mins > 0)
                {
                    result = hours.ToString() + "H " + mins.ToString() + "M";
                }
                else
                {
                    result = hours.ToString() + "H";
                }
            }
            else if (mins > 0)
            {
                int sec = Mathf.FloorToInt((float)time.TotalSeconds) - (mins * 60);
                if (sec > 0)
                {
                    result = mins.ToString() + "M " + sec.ToString() + "s";
                }
                else
                {
                    result = mins.ToString() + "M";
                }
            }
            else
            {
                result = ((int)time.TotalSeconds).ToString() + "s";
            }
            return result;
        }

        public static string SecondsToTimeFormat(int seconds)
        {
            return SecondsToTimeFormat(TimeSpan.FromSeconds(seconds));
        }

    }
}