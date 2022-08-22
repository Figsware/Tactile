using System;
using System.Linq;
using System.Reflection;
using Tactile.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tactile
{
    [CreateAssetMenu(fileName = "TactileConfig", menuName = "Tactile/Tactile Config", order = 0)]
    public class TactileConfigurationData : ScriptableObject
    {
        public InfoCard infoCard;
        public KeyValuePair<UnityEngine.Object>[] templateItems;
    }
}