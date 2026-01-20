using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterVents.Patches
{
    [HarmonyPatch(typeof(PingTracker), "Update")]
    internal static class PingTrackerPatch
    {
        public static void Postfix(PingTracker __instance)
        {
            __instance.text.text += "\nBetterVents: https://github.com/OBit508/BetterVents";
        }
    }
}
