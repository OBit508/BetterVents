using BetterMods;
using BetterVents.Components;
using Il2CppInterop.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterVents
{
    internal static class Helpers
    {
        private static Transform parent;
        private static PassiveButton ventButtonPrefab;
        public static Material ImpostorVentButtonMaterial;
        public static Sprite VentButton;
        public static Transform PrefabParent
        {
            get
            {
                if (parent == null)
                {
                    parent = new GameObject("Prefabs")
                    {
                        active = false,
                        transform =
                {
                    parent = AmongUsClient.Instance.transform,
                }
                    }.transform;
                }
                return parent;
            }
        }
        public static void LoadAssets()
        {
            MemoryStream ms = new MemoryStream();
            BetterVentsPlugin.Assembly.GetManifestResourceStream("BetterVents.Assets.bettervents_bundle").CopyTo(ms);
            ImpostorVentButtonMaterial = AssetBundle.LoadFromMemory(ms.ToArray()).LoadAsset("ImpostorVentButtonMaterial", Il2CppType.Of<Material>()).Cast<Material>().DontUnload();
            MemoryStream ms2 = new MemoryStream();
            BetterVentsPlugin.Assembly.GetManifestResourceStream("BetterVents.Assets.ventButton.png").CopyTo(ms2);
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            texture2D.LoadImage(ms2.ToArray());
            texture2D.Apply();
            VentButton = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100).DontUnload();
        }
        public static PassiveButton GetVentButtonPrefab()
        {
            if (ventButtonPrefab == null)
            {
                ventButtonPrefab = new GameObject().AddComponent<PassiveButton>();
                BoxCollider2D boxCollider2D = ventButtonPrefab.gameObject.AddComponent<BoxCollider2D>();
                boxCollider2D.isTrigger = true;
                boxCollider2D.size = new Vector2(1.2f, 0.85f);
                SpriteRenderer rend = ventButtonPrefab.gameObject.AddComponent<SpriteRenderer>();
                rend.sprite = Helpers.VentButton;
                ventButtonPrefab.OnMouseOver = new UnityEngine.Events.UnityEvent();
                ventButtonPrefab.OnMouseOut = new UnityEngine.Events.UnityEvent();
                ButtonRolloverHandler buttonRolloverHandler = ventButtonPrefab.gameObject.AddComponent<ButtonRolloverHandler>();
                buttonRolloverHandler.Target = rend;
                buttonRolloverHandler.OutColor = Color.white;
                buttonRolloverHandler.OverColor = Color.gray;
                ventButtonPrefab.gameObject.layer = 5;
                ventButtonPrefab.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
                ventButtonPrefab.transform.SetParent(PrefabParent);
            }
            return ventButtonPrefab;
        }
        public static VentHelper TryGetHelper(this Vent vent)
        {
            try
            {
                return VentHelper.ShipVents[vent];
            }
            catch
            {
                return vent.GetComponent<VentHelper>();
            }
        }
        public static T DontUnload<T>(this T obj) where T : UnityEngine.Object
        {
            ref T ptr = ref obj;
            ptr.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return obj;
        }
    }
}
