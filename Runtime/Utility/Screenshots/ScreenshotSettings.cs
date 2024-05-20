using System;
using System.IO;
using Tactile.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tactile.Utility.Screenshots
{
    [CreateAssetMenu(fileName = "ScreenshotSettings", menuName = "Tactile/Screenshot Settings", order = 0)]
    public class ScreenshotSettings : ScriptableObject
    {
        [SerializeField] private int width = 1920;
        [SerializeField] private int height = 1080;
        [SerializeField] private RectTransform overlay;
        
        public int Width => width;
        public int Height => height;
        public RectTransform Overlay => overlay;

        public Screenshotter CreateScreenshotter() => new(this);
    }
}