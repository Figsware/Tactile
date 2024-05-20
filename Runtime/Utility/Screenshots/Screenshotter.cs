using System;
using System.IO;
using Tactile.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tactile.Utility.Screenshots
{
    /// <summary>
    /// A screenshotter enables you to take screenshots from camera. It maintains a render texture to hold screenshots
    /// and can export them.
    /// </summary>
    public class Screenshotter : IDisposable
    {
        #region Fields

        /// <summary>
        /// The width (in pixels) of the screenshot.
        /// </summary>
        public int Width = 1920;

        /// <summary>
        /// The height (in pixels) of the screenshot.
        /// </summary>
        public int Height = 1080;

        /// <summary>
        /// An overlay prefab to add on top of the camera.
        /// </summary>
        public RectTransform CanvasOverlayPrefab;

        #endregion

        #region Properties

        /// <summary>
        /// The aspect ratio of the screenshot (width / height).
        /// </summary>
        public float AspectRatio => (float)Width / Height;

        /// <summary>
        /// The render texture used to capture screenshots.
        /// </summary>
        public RenderTexture ScreenshotRenderTexture => GetRenderTexture();

        /// <summary>
        /// Whether the screenshotter has taken a screenshot given its current configuration.
        /// </summary>
        public bool HasScreenshot { get; private set; } = false;

        #endregion

        # region Private Fields

        private RenderTexture _renderTexture;

        #endregion

        # region Constructors
        
        public Screenshotter()
        {
        }

        public Screenshotter(ScreenshotSettings settings)
        {
            UseScreenshotSettings(settings);
        }
        
        #endregion

        /// <summary>
        /// Applies settings from a ScreenshotSettings ScriptableObject to this Screenshotter.
        /// </summary>
        /// <param name="settings">The new settings to apply</param>
        public void UseScreenshotSettings(ScreenshotSettings settings)
        {
            Height = settings.Height;
            Width = settings.Width;
            CanvasOverlayPrefab = settings.Overlay;
        }

        /// <summary>
        /// Gets the render texture used by this Screenshotter.
        /// </summary>
        /// <returns>The render texture used by this Screenshotter</returns>
        public RenderTexture GetRenderTexture()
        {
            // If we already have a good render texture, return it.
            if (_renderTexture && _renderTexture.width == Width && _renderTexture.height == Height)
                return _renderTexture;

            if (_renderTexture)
            {
                Object.DestroyImmediate(_renderTexture);
            }

            HasScreenshot = false;
            _renderTexture = new RenderTexture(Width, Height, 32);

            return _renderTexture;
        }

        /// <summary>
        /// Captures a screenshot from the provided camera.
        /// </summary>
        /// <param name="camera">The camera to take a screenshot from.</param>
        public void Screenshot(Camera camera)
        {
            GameObject overlayCanvasGameObject = null;

            if (CanvasOverlayPrefab)
            {
                overlayCanvasGameObject = new GameObject();

                var canvas = overlayCanvasGameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = camera;
                var overlayInstance = Object.Instantiate(CanvasOverlayPrefab, overlayCanvasGameObject.transform);
                overlayInstance.Stretch();
            }

            var renderTexture = GetRenderTexture();
            camera.RenderToRenderTexture(renderTexture);

            if (overlayCanvasGameObject)
            {
                Object.DestroyImmediate(overlayCanvasGameObject);
            }

            HasScreenshot = true;
        }

        /// <summary>
        /// Encodes the current screenshot t
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NoScreenshotException">If a screenshot has not been captured yet.</exception>
        public byte[] EncodeToPNG()
        {
            ThrowIfNoScreenshot();

            var png = ScreenshotRenderTexture.EncodeToPNG();
            return png;
        }

        /// <summary>
        /// Saves the current screenshot as a PNG to the specified path.
        /// </summary>
        /// <param name="path">The path to save the PNG</param>
        public void SaveToPNG(string path)
        {
            ThrowIfNoScreenshot();

            var png = EncodeToPNG();
            File.WriteAllBytes(path, png);
        }

        /// <summary>
        /// Frees up resources used by this Screenshotter.
        /// </summary>
        public void Clear()
        {
            if (_renderTexture)
                Object.DestroyImmediate(_renderTexture);

            _renderTexture = null;
            HasScreenshot = false;
        }

        public void Dispose()
        {
            Clear();
        }

        /// <summary>
        /// Throws a NoScreenshotException if a screenshot hasn't been captured yet. 
        /// </summary>
        /// <exception cref="NoScreenshotException">Occurs if a screenshot hasn't been captured yet.</exception>
        private void ThrowIfNoScreenshot()
        {
            if (!HasScreenshot)
                throw new NoScreenshotException();
        }

        public class NoScreenshotException : Exception
        {
            public NoScreenshotException() : base("No screenshot has been taken by this Screenshotter yet!")
            {
            }
        }
    }
}