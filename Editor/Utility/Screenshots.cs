using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor
{
    public static class Screenshots
    {
        [MenuItem("Tactile/Screenshot")]
        public static void CaptureScreenshot()
        {
            var view = SceneView.currentDrawingSceneView ? SceneView.currentDrawingSceneView : SceneView.lastActiveSceneView;
            if (view != null)
            {
                Camera camera = view.camera;
                RenderTexture renderTexture = new RenderTexture(1920, 1080, 32);
                camera.targetTexture = renderTexture;
                RenderTexture.active = renderTexture;

                camera.Render();

                Texture2D image = new Texture2D(renderTexture.width, renderTexture.height);
                image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                image.Apply();

                RenderTexture.active = null;

                byte[] bytes = image.EncodeToPNG();
                UnityEngine.Object.DestroyImmediate(image);

                Directory.CreateDirectory(Path.Join(Application.persistentDataPath, "Screenshots/"));
                string path = Application.persistentDataPath + "/Screenshots/" +
                              DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".png";
                File.WriteAllBytes(path, bytes);
                Debug.Log($"Saved screenshot to {path}");
                
                // Present the captured screenshot.
                EditorUtility.OpenWithDefaultApp(path);
            }
        }
    }
}