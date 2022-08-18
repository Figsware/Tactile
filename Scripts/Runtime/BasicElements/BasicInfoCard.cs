using System.Collections;
using Tactile.UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Tactile.BasicElements
{
    /// <summary>
    /// A basic flat UI card for displaying information.
    /// </summary>
    [AddComponentMenu("")]
    public class BasicInfoCard: InfoCard
    {
        /// <summary>
        /// The amount of time to open/close the info card.
        /// </summary>
        [SerializeField] private float openCloseTime;

        /// <summary>
        /// The text label to display the message content.
        /// </summary>
        [SerializeField] private TextMeshProUGUI textLabel;

        /// <summary>
        /// The background image of the cell.
        /// </summary>
        [SerializeField] private Image backgroundImage;

        /// <summary>
        /// The image used to shade the cell center.
        /// </summary>
        [SerializeField] private Image shadeImage;

        /// <summary>
        /// The parent transform of the InfoCard.
        /// </summary>
        [SerializeField] private Transform cardTransform;

        private static Vector3 OpenScale => Vector3.one;
        private static Vector3 ClosedScale => new(0f, 1f, 1f);

        public override Color Color
        {
            set => SetColor(value);
        }

        public override string Message
        {
            set => textLabel.text = value;
        }
        
        protected override IEnumerator OpenCoroutine()
        {
            cardTransform.localScale = ClosedScale;
            yield return StartCoroutine(cardTransform.LinearScaleCoroutine(OpenScale, openCloseTime));
        }

        protected override IEnumerator CloseCoroutine()
        {
            cardTransform.localScale = OpenScale;
            yield return StartCoroutine(cardTransform.LinearScaleCoroutine(ClosedScale, openCloseTime));
        }

        void SetColor(Color color)
        {
            // Apply background image color
            backgroundImage.color = color;
            
            // Determine content color based on background color.
            Color contentColor = color.ChooseTextColorForBackgroundColor(Color.white, Color.black);
            
            // Apply text color
            textLabel.color = contentColor;
        }
    }
}