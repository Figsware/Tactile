using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.Tools
{
    [AddComponentMenu("Tactile/Tools/Digital Clock")]
    [ExecuteAlways]
    public class DigitalClock : MonoBehaviour
    {
        /// <summary>
        /// An event that is triggered when there is new clock text.
        /// </summary>
        public UnityEvent<string> onNewClockText;

        /// <summary>
        /// The format to display the time.
        /// </summary>
        public string timeFormat = "h:mm tt";

        // Update is called once per frame
        void Update()
        {
            DateTime now = DateTime.Now;
            onNewClockText.Invoke(now.ToString(timeFormat));
        }
    }
}