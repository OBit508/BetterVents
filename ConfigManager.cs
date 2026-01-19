using BepInEx.Configuration;
using Sentry.Protocol.Envelopes;
using Sentry.Unity.NativeUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterVents
{
    internal static class ConfigManager
    {
        public static string DataPath = Path.Combine(Application.dataPath, "BetterVentsData");
        public static BetterVentsData Data;
        public static Color VentColor;
        public static void Initialize(ConfigFile config)
        {
            if (ColorUtility.TryParseHtmlString(config.Bind("Configurations", "Vent Color (HEX)", "#006eff").Value, out Color c))
            {
                VentColor = c;
            }
            else
            {
                VentColor = new Color32(0, 110, 255, byte.MaxValue);
            }
            if (File.Exists(DataPath))
            {
                Data = JsonSerializer.Deserialize<BetterVentsData>(File.ReadAllText(DataPath));
                return;
            }
            Data = new BetterVentsData()
            {
                Ships = new Dictionary<string, ShipData>()
            };
            File.WriteAllText(DataPath, JsonSerializer.Serialize(Data));
        }
        public static void Update()
        {
            File.WriteAllText(DataPath, JsonSerializer.Serialize(Data));
        }
        public class BetterVentsData
        {
            public Dictionary<string, ShipData> Ships { get; set; }
        }
        public class ShipData
        {
            public Dictionary<string, VentData> Vents { get; set; }
        }
        public class VentData
        {
            public List<string> Vents { get; set; }
        }
    }
}
