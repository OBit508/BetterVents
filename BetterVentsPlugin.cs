using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BetterVents;
using BetterVents.Components;
using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.Injection;
using InnerNet;
using TMPro;
using UnityEngine;

namespace BetterMods
{
	[BepInProcess("Among Us.exe")]
	[BepInPlugin("BetterVents", "BetterVents", "0.1.0")]
    public class BetterVentsPlugin : BasePlugin
    {
        public static Assembly Assembly = Assembly.GetExecutingAssembly();
        public static ManualLogSource Logger;
		public override void Load()
		{
            Logger = Log;
            ClassInjector.RegisterTypeInIl2Cpp<VentButtomComp>();
            ClassInjector.RegisterTypeInIl2Cpp<VentHelper>();
            ClassInjector.RegisterTypeInIl2Cpp<MapVentButtonComp>();
            Helpers.LoadAssets();
            ConfigManager.Initialize(Config);
            new Harmony("BetterVents").PatchAll();
        }
    }
}
