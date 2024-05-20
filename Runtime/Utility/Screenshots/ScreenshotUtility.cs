using UnityEngine;
using Object = UnityEngine.Object;

namespace Tactile.Utility.Screenshots
{
    public static class ScreenshotUtility
    {
        public static void RenderToRenderTexture(this Camera camera, RenderTexture toRenderTexture)
        {
            var prevCameraTargetTexture = camera.targetTexture;

            camera.targetTexture = toRenderTexture;
            camera.Render();

            camera.targetTexture = prevCameraTargetTexture;
        }

        public static Texture2D RenderToTexture2D(this RenderTexture renderTexture)
        {
            var prevActiveRenderTexture = RenderTexture.active;

            RenderTexture.active = renderTexture;
            var image = new Texture2D(renderTexture.width, renderTexture.height);
            image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            image.Apply();

            RenderTexture.active = prevActiveRenderTexture;

            return image;
        }

        public static byte[] EncodeToPNG(this RenderTexture renderTexture)
        {
            var tex = renderTexture.RenderToTexture2D();
            var pngBytes = tex.EncodeToPNG();
            Object.DestroyImmediate(tex);

            return pngBytes;
        }
    }
}