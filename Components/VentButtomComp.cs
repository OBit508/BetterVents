using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterVents.Components
{
    internal class VentButtomComp : MonoBehaviour
    {
        public VentButton Button;
        public Color Current;
        public void Update()
        {
            if (ConfigManager.VentColor != Current)
            {
                Button.graphic.material.SetColor("_ReplaceColor", ConfigManager.VentColor);
                Button.buttonLabelText.SetOutlineColor(ConfigManager.VentColor);
                Current = ConfigManager.VentColor;
            }
        }
    }
}
