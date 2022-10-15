using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace Tactile.Tools
{
    [AddComponentMenu("Tactile/Tools/Digital Clock")]
    [ExecuteAlways]
    public class DigitalClock : MonoBehaviour
    {
        /// <summary>
        /// The label to display the time.
        /// </summary>
        public TextMeshProUGUI clockLabel;

        /// <summary>
        /// The format to display the time.
        /// </summary>
        public string timeFormat = "h:mm tt";

        private void Awake()
        {
            Debug.Assert(clockLabel, "Clock label is missing!");
        }

        // Update is called once per frame
        void Update()
        {
            DateTime now = DateTime.Now;

            if (clockLabel)
                clockLabel.text = now.ToString(timeFormat);
        }
    }
}