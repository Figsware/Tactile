using Tactile.UI.Menu;
using Tactile.Utility;
using UnityEngine;

namespace Tactile
{
    [CreateAssetMenu(fileName = "TactileConfig", menuName = "Tactile/Tactile Config", order = 0)]
    public class TactileConfigurationData : ScriptableObject
    {
        public InfoCard infoCard;
        public Arrow arrow;
    }
}