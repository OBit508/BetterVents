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
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void StartPostfix(MapBehaviour __instance)
        {
            foreach (Vent vent in ShipStatus.Instance.AllVents)
            {
                Vector3 vector = vent.transform.position;
                vector /= ShipStatus.Instance.MapScale;
                vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
                vector.z = -1f;
                SpriteRenderer spriteRenderer = new GameObject(vent.name)
                {
                    layer = 5,
                    transform =
                    {
                        parent = __instance.HerePoint.transform.parent,
                        localPosition = vector
                    }
                }.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Helpers.VentButton;
                BoxCollider2D boxCollider2D = spriteRenderer.gameObject.AddComponent<BoxCollider2D>();
                boxCollider2D.isTrigger = true;
                ButtonRolloverHandler buttonRolloverHandler = spriteRenderer.gameObject.AddComponent<ButtonRolloverHandler>();
                buttonRolloverHandler.Target = spriteRenderer;
                buttonRolloverHandler.OverColor = Color.gray;
                PassiveButton passiveButton = spriteRenderer.gameObject.AddComponent<PassiveButton>();
                passiveButton.OnClick.AddListener(new Action(delegate
                {

                }));
            }
        }
    }
}
