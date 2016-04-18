using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    class TimedObjectFlag : MonoBehaviour
    {
        private Dictionary<string, float> flags = new Dictionary<string, float>();
        public bool HasFlag(string flag) { return flags.ContainsKey(flag); }
        public void SetFlag(string flag, float lifetime)
        {
            flags.Add(flag, Time.time + lifetime);
        }

        private void Update()
        {
            var expired = flags.Where(kvp => Time.time > kvp.Value);

            foreach(KeyValuePair<string, float> kvp in expired)
            {
                flags.Remove(kvp.Key);
            }
        }
    }
}
