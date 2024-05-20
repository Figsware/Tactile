using System;
using TMPro;
using UnityEngine;

namespace Tactile.Utility.Screenshots
{
    [ExecuteAlways]
    public class ScreenshotDebugInformation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI debugText;
        
        private void Awake()
        {
            debugText.text = $"{Application.productName}: {Application.version} - {DateTime.Now}";
        }
    }
}