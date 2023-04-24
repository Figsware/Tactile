using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

namespace Tactile.Gadgets
{
    /// <summary>
    /// Helps display console messages in the runtime.
    /// </summary>
    [AddComponentMenu("Tactile/Gadgets/Console Messages")]
    public class ConsoleMessages : MonoBehaviour
    {

        public bool OnlyEnableIfDebugBuild = true;
        public TextMeshProUGUI consoleText;
        public int MessageBufferSize;
        public bool ShowStackTrace;
        private List<string> messages = new List<string>();


        // Start is called before the first frame update
        void OnEnable()
        {
            Application.logMessageReceived += AddLogMessage;
            messages.Clear();
            messages.Capacity = MessageBufferSize;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= AddLogMessage;
        }
        // Update is called once per frame
        void Update()
        {
            if (OnlyEnableIfDebugBuild)
            {
                consoleText.enabled = Debug.isDebugBuild;
            }
            else
            {
                consoleText.enabled = true;
            }
        }

        void AddLogMessage(string logString, string stackTrace, LogType type)
        {
            string msg = DateTime.Now.ToString("[HH:mm:ss] ") + type.ToString() + ": " + logString + "\n";
            if (ShowStackTrace)
                msg += stackTrace + "\n";
            if (messages.Count > MessageBufferSize)
            {
                messages.RemoveAt(0);
            }

            switch (type)
            {
                case LogType.Error:
                    msg = "<color=\"red\">" + msg + "</color>";
                    break;
                case LogType.Warning:
                    msg = "<color=\"yellow\">" + msg + "</color>";
                    break;
                default:
                    break;
            }
            messages.Add(msg);

            consoleText.text = "";
            foreach (string line in messages)
            {
                consoleText.text += line;
            }
        }
    }
}