using BetterVents.Components;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterVents.Patches
{
    [HarmonyPatch(typeof(Vent))]
    internal static class VentPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void StartPostfix(Vent __instance)
        {
            if (ShipStatus.Instance == null)
            {
                return;
            }
            if (VentHelper.ArrowPrefab == null)
            {
                VentHelper.ArrowPrefab = GameObject.Instantiate(__instance.Buttons[0]);
                VentHelper.ArrowPrefab.transform.SetParent(AmongUsClient.Instance.transform);
            }
            VentHelper ventHelper = __instance.gameObject.AddComponent<VentHelper>();
            ventHelper.vent = __instance;
            string shipName = ShipStatus.Instance.GetType().FullName;
            ConfigManager.ShipData ship;
            if (!ConfigManager.Data.Ships.TryGetValue(shipName, out ship))
            {
                ship = new ConfigManager.ShipData() { Vents = new Dictionary<string, ConfigManager.VentData>() };
                ConfigManager.Data.Ships.Add(shipName, ship);
            }
            ConfigManager.VentData vent;
            if (!ship.Vents.TryGetValue(__instance.name, out vent))
            {
                vent = new ConfigManager.VentData()
                {
                    Vents = new List<string>()
                };
                vent.Vents.Add(__instance.Center == null ? "" : __instance.Center.name);
                vent.Vents.Add(__instance.Left == null ? "" : __instance.Left.name);
                vent.Vents.Add(__instance.Right == null ? "" : __instance.Right.name);
                ship.Vents.Add(__instance.name, vent);
            }
            List<Vent> shipVents = ShipStatus.Instance.AllVents.ToList();
            foreach (string v in vent.Vents)
            {
                Vent ve = shipVents.FirstOrDefault(ve => ve.name == v);
                if (ve != null)
                {
                    ventHelper.Vents.Add(ve);
                }
            }
            ConfigManager.Update();
        }
        [HarmonyPatch("SetOutline")]
        [HarmonyPrefix]
        public static bool SetOutlinePrefix(Vent __instance, [HarmonyArgument(0)] bool on, [HarmonyArgument(1)] bool mainTarget)
        {
            if (on)
            {
                __instance.myRend.material.SetFloat("_Outline", 1f);
                __instance.myRend.material.SetColor("_OutlineColor", ConfigManager.VentColor);
            }
            else
            {
                __instance.myRend.material.SetFloat("_Outline", 0f);
            }
            if (mainTarget)
            {
                __instance.myRend.material.SetColor("_AddColor", new Color(Mathf.Clamp01(ConfigManager.VentColor.r * 0.5f), Mathf.Clamp01(ConfigManager.VentColor.g * 0.5f), Mathf.Clamp01(ConfigManager.VentColor.b * 0.5f), 1f));
            }
            else
            {
                __instance.myRend.material.SetColor("_AddColor", new Color(0f, 0f, 0f, 0f));
            }
            return false;
        }
        [HarmonyPatch("CanUse")]
        [HarmonyPrefix]
        public static bool CanUsePrefix(Vent __instance, NetworkedPlayerInfo pc, ref bool canUse, ref bool couldUse, ref float __result)
        {
            float num = float.MaxValue;
            PlayerControl @object = pc.Object;
            Vector3 center = @object.Collider.bounds.center;
            Vector3 position = __instance.transform.position;
            num = Vector2.Distance(center, position);
            couldUse = true;
            canUse = num <= __instance.UsableDistance && !PhysicsHelpers.AnythingBetween(@object.Collider, center, position, Constants.ShipOnlyMask, false);
            __result = num;
            return false;
        }
        [HarmonyPatch("SetButtons")]
        [HarmonyPrefix]
        public static bool SetButtonsPrefix(Vent __instance, [HarmonyArgument(0)] bool enabled)
        {
            if (enabled)
            {
                List<(Vent vent, ButtonBehavior button, GameObject clean)> entries = new List<(Vent vent, ButtonBehavior button, GameObject clean)>();
                for (int i = 0; i < __instance.Buttons.Length; i++)
                {
                    Vent v = __instance.NearbyVents[i];
                    if (v)
                    {
                        entries.Add((v, __instance.Buttons[i], __instance.CleaningIndicators[i]));
                    }
                }
                VentilationSystem ventilationSystem = ShipStatus.Instance.Systems[SystemTypes.Ventilation].Cast<VentilationSystem>();
                for (int i = 0; i < entries.Count; i++)
                {
                    (Vent vent, ButtonBehavior button, GameObject clean) e = entries[i];
                    ButtonBehavior buttonBehavior = e.button;
                    buttonBehavior.gameObject.SetActive(true);
                    __instance.ToggleNeighborVentBeingCleaned(ventilationSystem.IsVentCurrentlyBeingCleaned(e.vent.Id), e.button, e.clean);
                    Vector3 vector2 = (e.vent.transform.position - __instance.transform.position).normalized * (0.7f + __instance.spreadShift);
                    vector2.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
                    vector2.y -= 0.08f;
                    vector2.z = -10f;
                    int offsetIndex = i - (entries.Count / 2);
                    if (entries.Count % 2 == 0)
                    {
                        offsetIndex += i < (entries.Count / 2) ? 0 : 1;
                    }
                    vector2 = vector2.RotateZ(offsetIndex * __instance.spreadAmount);
                    buttonBehavior.transform.localPosition = vector2;
                    buttonBehavior.transform.LookAt2d(e.vent.transform);
                    buttonBehavior.transform.Rotate(0f, 0f, offsetIndex * __instance.spreadAmount);
                }
            }
            else
            {
                for (int i = 0; i < __instance.Buttons.Count; i++)
                {
                    __instance.Buttons[i].gameObject.SetActive(false);
                }
            }
            return false;
        }
    }
}
