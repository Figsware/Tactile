using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tactile.UI
{
    /// <summary>
    /// A customizable UI progress bar.
    /// </summary>
    [ExecuteAlways]
    public class ProgressBar : MonoBehaviour
    {
        #region Inspector Variables
        /// <summary>
        /// The current progress of the bar.
        /// </summary>
        [Range(0, 1f)]
        public float progress = 0.5f;

        /// <summary>
        /// The color used for the progress bar.
        /// </summary>
        public Color progressColor;

        /// <summary>
        /// The color used in the background.
        /// </summary>
        public Color backgroundColor;

        /// <summary>
        /// The image representing the background.
        /// </summary>
        public Image backgroundImage;

        /// <summary>
        /// The image representing the progress bar.
        /// </summary>
        public Image progressImage;
        #endregion

        // Update is called once per frame
        protected virtual void Update()
        {
            if (backgroundImage != null && progressImage != null)
            {
                // Set colors
                backgroundImage.color = backgroundColor;
                progressImage.color = progressColor;

                progressImage.rectTransform.anchorMax = new Vector2(Mathf.Clamp(progress, 0, 1f), 1f);
            }
        }
    }
}