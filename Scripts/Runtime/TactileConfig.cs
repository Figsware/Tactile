using System;
using Tactile.UI;
using UnityEngine;

namespace Tactile
{
    [ExecuteInEditMode]
    public class TactileConfig : MonoBehaviour
    {
        public bool addToDontDestroyOnLoad = true;
        public Color teamColor;

        [SerializeField]
        private TactileConfigurationData config;

        private static TactileConfigurationData sharedConfig;

        public static TactileConfigurationData Config => GetTactileConfig();

        private void Awake()
        {
            sharedConfig = config;
            Debug.Assert(config, "Could not load Tactile configuration!");
            
            if (Application.isPlaying && addToDontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        public static InfoCard CreateInfoCard() => Instantiate(sharedConfig.infoCard);

        static TactileConfigurationData GetTactileConfig()
        {
            if (sharedConfig == null && FindObjectOfType<TactileConfig>() is { } provider)
            {
                sharedConfig = provider.config;
            }

            return sharedConfig;
        }
    }   
}