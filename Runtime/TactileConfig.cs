using System;
using Tactile.UI;
using UnityEngine;

namespace Tactile
{
    public class TactileConfig : MonoBehaviour
    {
        public bool addToDontDestroyOnLoad = true;
        
        [SerializeField]
        private TactileConfigurationData config;

        private static TactileConfigurationData sharedConfig;
        private void Awake()
        {
            sharedConfig = config;
            Debug.Assert(config, "Could not load Tactile configuration!");
            
            if (addToDontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        public static InfoCard CreateInfoCard() => Instantiate(sharedConfig.infoCard);
    }   
}