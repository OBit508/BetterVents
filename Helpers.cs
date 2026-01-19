using BetterMods;
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
        public static Material ImpostorVentButtonMaterial;
        public static Sprite VentButton;
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
        public static T DontUnload<T>(this T obj) where T : UnityEngine.Object
        {
            ref T ptr = ref obj;
            ptr.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return obj;
        }
    }
}
