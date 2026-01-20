using BetterVents.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterVents.Components
{
    internal class MapVentButtonComp : MonoBehaviour
    {
        public Dictionary<VentHelper, LineRenderer> Rends = new Dictionary<VentHelper, LineRenderer>();
        public VentHelper current;
        public int lastCount;
        public void Update()
        {
            if (lastCount != current.Vents.Count)
            {
                foreach (LineRenderer r in Rends.Values)
                {
                    GameObject.Destroy(r.gameObject);
                }
                Rends.Clear();
                foreach (Vent v in current.Vents)
                {
                    LineRenderer lr = new GameObject("line")
                    {
                        layer = 5,
                        transform =
                        {
                            parent = transform,
                            localPosition = new Vector3(0, 0, 0.1f)
                        }
                    }.AddComponent<LineRenderer>();
                    lr.positionCount = 2;
                    lr.startWidth = 0.03f;
                    lr.endWidth = 0.03f;
                    lr.material = new Material(Shader.Find("Sprites/Default"));
                    lr.startColor = ConfigManager.VentColor;
                    lr.endColor = ConfigManager.VentColor;
                    Rends.Add(v.GetComponent<VentHelper>(), lr);
                }
                lastCount = current.Vents.Count;
            }
            foreach (KeyValuePair<VentHelper, LineRenderer> pair in Rends)
            {
                pair.Value.startColor = ConfigManager.VentColor;
                pair.Value.endColor = ConfigManager.VentColor;
                pair.Value.SetPosition(0, transform.position);
                pair.Value.SetPosition(1, pair.Key.MapButton.transform.position);
            }
        }
    }
}
