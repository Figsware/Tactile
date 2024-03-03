using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactile.UI
{
    /// <summary>
    /// A customizable UI progress bar.
    /// </summary>
    [ExecuteAlways]
    public class CircularProgressBar : ProgressBar
    {
        // Update is called once per frame
        protected override void Update()
        {
            // Set colors
            backgroundImage.color = backgroundColor;
            progressImage.color = progressColor;

            progressImage.fillAmount = progress;
        }
    }
}