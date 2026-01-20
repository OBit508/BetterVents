using BetterMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterVents.Components
{
    internal class VentHelper : MonoBehaviour
    {
        public static Dictionary<Vent, VentHelper> ShipVents = new Dictionary<Vent, VentHelper>();
        public ButtonBehavior ArrowPrefab;
        public List<Vent> Vents = new List<Vent>();
        public Vent vent;
        public PassiveButton MapButton;
        public void Start()
        {
            foreach (ButtonBehavior buttonBehavior in vent.Buttons)
            {
                GameObject.Destroy(buttonBehavior.gameObject);
            }
            List<ButtonBehavior> buttons = new List<ButtonBehavior>();
            List<GameObject> cleaningIndicators = new List<GameObject>();
            foreach (Vent vent in Vents)
            {
                ButtonBehavior button = GameObject.Instantiate<ButtonBehavior>(ArrowPrefab, transform);
                buttons.Add(button);
                cleaningIndicators.Add(button.transform.GetChild(0).gameObject);
                button.OnClick.RemoveAllListeners();
                button.OnClick.AddListener(new Action(delegate
                {
                    string errorText;
                    if (!this.vent.TryMoveToVent(vent, out errorText))
                    {
                        BetterVentsPlugin.Logger.LogError("Local Player failed to move to " + vent.name + " because of " + errorText);
                    }
                }));
            }
            vent.Buttons = buttons.ToArray();
            vent.CleaningIndicators = cleaningIndicators.ToArray();
            vent.SetButtons(Vent.currentVent == vent);
        }
        public void Update()
        {
            if (Vents.Count != vent.Buttons.Count)
            {
                Start();
            }
        }
    }
}
