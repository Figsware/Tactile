﻿using UnityEngine;

namespace Tactile.Gadgets.InspectorUtility
{
    public class DebugInspectorUtility: MonoBehaviour
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}