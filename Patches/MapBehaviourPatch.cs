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
    [HarmonyPatch(typeof(MapBehaviour))]
    internal static class MapBehaviourPatch
    {
        public static MapVentButtonComp Selected;
        public static LineRenderer Line;
        [HarmonyPatch("FixedUpdate")]
        [HarmonyPostfix]
        public static void FixedUpdatePostfix(MapBehaviour __instance)
        {
            if (Selected != null)
            {
                Line.startColor = ConfigManager.VentColor;
                Line.endColor = ConfigManager.VentColor;
                Line.SetPosition(0, Selected.transform.parent.position);
                Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                vec.z = __instance.HerePoint.transform.position.z;
                Line.SetPosition(1, vec);
            }
        }
        [HarmonyPatch("Show")]
        [HarmonyPostfix]
        public static void ShowPostfix()
        {
            if (Selected != null)
            {
                Selected = null;
                ButtonRolloverHandler b = Selected.transform.parent.GetComponent<ButtonRolloverHandler>();
                b.enabled = true;
                b.Target.color = b.OutColor;
                Selected = null;
                Line.positionCount = 0;
                Line.gameObject.SetActive(false);
            }
        }
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void StartPostfix(MapBehaviour __instance)
        {
            Selected = null;
            Line = new GameObject("line")
            {
                layer = 5,
            }.AddComponent<LineRenderer>();
            Line.startWidth = 0.03f;
            Line.endWidth = 0.03f;
            Line.material = new Material(Shader.Find("Sprites/Default"));
            foreach (Vent vent in ShipStatus.Instance.AllVents)
            {
                Vector3 vector = vent.transform.position;
                vector /= ShipStatus.Instance.MapScale;
                vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
                vector.z = -1f;
                PassiveButton passiveButton = GameObject.Instantiate(Helpers.GetVentButtonPrefab(), __instance.HerePoint.transform.parent);
                passiveButton.transform.localPosition = vector;
                ButtonRolloverHandler buttonRolloverHandler = passiveButton.GetComponent<ButtonRolloverHandler>();
                MapVentButtonComp mapVentButtonComp = new GameObject("rends")
                {
                    transform =
                    {
                        parent = passiveButton.transform,
                        localPosition = Vector3.zero
                    }
                }.AddComponent<MapVentButtonComp>();
                mapVentButtonComp.current = vent.GetComponent<VentHelper>();
                mapVentButtonComp.current.MapButton = passiveButton;
                mapVentButtonComp.gameObject.SetActive(false);
                passiveButton.OnMouseOver.AddListener(new Action(delegate
                {
                    mapVentButtonComp.gameObject.SetActive(true);
                }));
                passiveButton.OnMouseOut.AddListener(new Action(delegate
                {
                    mapVentButtonComp.gameObject.SetActive(false);
                }));
                passiveButton.OnClick.AddListener(new Action(delegate
                {
                    if (Selected == null)
                    {
                        Selected = mapVentButtonComp;
                        buttonRolloverHandler.enabled = false;
                        buttonRolloverHandler.Target.color = buttonRolloverHandler.OverColor;
                        Line.positionCount = 2;
                        Line.transform.SetParent(passiveButton.transform);
                        Line.transform.localPosition = new Vector3(0, 0, -0.1f);
                        Line.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (Selected != mapVentButtonComp)
                        {
                            if (!Selected.current.Vents.Contains(vent))
                            {
                                Selected.current.Vents.Add(vent);
                                ConfigManager.Data.Ships[ShipStatus.Instance.GetType().FullName].Vents[Selected.current.vent.name].Vents.Add(vent.name);
                                ConfigManager.Update();
                            }
                            else
                            {
                                Selected.current.Vents.Remove(vent);
                                ConfigManager.Data.Ships[ShipStatus.Instance.GetType().FullName].Vents[Selected.current.vent.name].Vents.Remove(vent.name);
                                ConfigManager.Update();
                            }
                        }
                        ButtonRolloverHandler b = Selected.transform.parent.GetComponent<ButtonRolloverHandler>();
                        b.enabled = true;
                        b.Target.color = b.OutColor;
                        Selected = null;
                        Line.positionCount = 0;
                        Line.gameObject.SetActive(false);
                    }
                }));
            }
        }
    }
}
