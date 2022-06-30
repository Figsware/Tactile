using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tactile.Utility
{
    /// <summary>
    /// Adds helper functions that expose the color properties of a Selectable.
    /// In this way, you can use UnityEvents to set the color of selectables.
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    [AddComponentMenu("Tactile/Utility/Selectable Helper")]
    public class SelectableHelper : MonoBehaviour
    {

        /// <summary>
        /// Sets whether the interactable is disabled.
        /// </summary>
        /// <param name="disabled"></param>
        public void SetDisabled(bool disabled)
        {
            GetSelectable().interactable = !disabled;
        }

        /// <summary>
        /// Sets the normal color of the Selectable.
        /// </summary>
        /// <param name="color">The new color</param>
        public void SetNormalColor(Color color)
        {
            var block = GetColorBlock();
            block.normalColor = color;
            SetColorBlock(block);
        }

        /// <summary>
        /// Sets the highlighted color of the Selectable.
        /// </summary>
        /// <param name="color">The new color</param>
        public void SetHighlightedColor(Color color)
        {
            var block = GetColorBlock();
            block.highlightedColor = color;
            SetColorBlock(block);
        }

        /// <summary>
        /// Sets the pressed color of the Selectable.
        /// </summary>
        /// <param name="color">The new color</param>
        public void SetPressedColor(Color color)
        {
            var block = GetColorBlock();
            block.pressedColor = color;
            SetColorBlock(block);
        }

        /// <summary>
        /// Sets the selected color of the Selectable.
        /// </summary>
        /// <param name="color">The new color</param>
        public void SetSelectedColor(Color color)
        {
            var block = GetColorBlock();
            block.selectedColor = color;
            SetColorBlock(block);
        }

        /// <summary>
        /// Sets the disabled color of the Selectable.
        /// </summary>
        /// <param name="color">The new color</param>
        public void SetDisabledColor(Color color)
        {
            var block = GetColorBlock();
            block.disabledColor = color;
            SetColorBlock(block);
        }

        /// <summary>
        /// Gets the color block of the attached Selectable.
        /// </summary>
        /// <returns>The ColorBlock</returns>
        private ColorBlock GetColorBlock()
        {
            return GetSelectable().colors;
        }

        /// <summary>
        /// Sets the color block of the attached Selectable.
        /// </summary>
        /// <param name="block">The new block to set</param>
        private void SetColorBlock(ColorBlock block)
        {
            GetSelectable().colors = block;
            if (gameObject.activeInHierarchy)
                StartCoroutine(NoColorTransition());
        }

        /// <summary>
        /// Gets the attached Selectable.
        /// </summary>
        /// <returns>The Selectable</returns>
        private Selectable GetSelectable()
        {
            return GetComponent<Selectable>();
        }

        /// <summary>
        /// Forces the selectable to immediately set any new colors by temporarily
        /// setting fade duration to zero for one frame.
        /// </summary>
        /// <returns></returns>
        private IEnumerator NoColorTransition()
        {
            Selectable selectable = GetSelectable();
            ColorBlock tempBlock = selectable.colors;
            float prevDuration = tempBlock.fadeDuration;

            // Only run if the fade duration isn't zero, which means that either
            // there is no fade duration to begin with or a NoColorTransition is
            // already running.
            if (prevDuration != 0)
            {
                // Create a temp color block with no fade duration.
                tempBlock.fadeDuration = 0;
                selectable.colors = tempBlock;

                // Wait a frame.
                yield return null;

                // Reset the color block to original. We need to grab the color block
                // again since it might have changed during the transition.
                ColorBlock currColors = GetColorBlock();
                currColors.fadeDuration = prevDuration;
                selectable.colors = currColors;
            }
        }
    }
}