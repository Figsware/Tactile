using System;
using System.Windows.Forms;
using Tactile.Utility;
using Tactile.Utility.Screenshots;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;
using System.Drawing;
using System.IO;

namespace Tactile.Editor.Gadgets
{
    public class ScreenshotWindow : EditorWindow
    {
        private Camera _sceneCamera;
        private ScreenshotSettings _screenshotSettings;
        private Texture2D _screenshotTexture;
        private Screenshotter _screenshotter = new();
        
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            _screenshotSettings = (ScreenshotSettings) EditorGUILayout.ObjectField("Screenshot Settings", _screenshotSettings,
                typeof(ScreenshotSettings), true);
            _sceneCamera = (Camera)EditorGUILayout.ObjectField("Camera", _sceneCamera, typeof(Camera), true);
            if (!_sceneCamera)
            {
                EditorGUILayout.HelpBox("The Scene View camera will be used to take the screenshot.", MessageType.Info);
            }
            
            if (_screenshotSettings)
            {
                var rect = GUILayoutUtility.GetAspectRect(_screenshotter.AspectRatio);
                GUI.DrawTexture(rect.Padding(16), _screenshotter.ScreenshotRenderTexture);
                
                if (GUILayout.Button("Screenshot"))
                {
                   Screenshot();
                }

                GUI.enabled = _screenshotter.HasScreenshot;
                if (GUILayout.Button("Copy to Clipboard"))
                {
                    CopyToClipboard();
                }
                
                if (GUILayout.Button("Save to PNG"))
                {
                    SaveToPNG();   
                }
            }
            
            EditorGUILayout.EndVertical();
        }

        private void Screenshot()
        {
            _screenshotter.UseScreenshotSettings(_screenshotSettings);
            
            var camera = _sceneCamera;
            if (!camera)
            {
                var view = SceneView.currentDrawingSceneView  
                    ? SceneView.currentDrawingSceneView 
                    : SceneView.lastActiveSceneView;

                camera = view.camera;
            }
                    
            _screenshotter.Screenshot(camera);
        }

        private void CopyToClipboard()
        {
            Image image;
            var png = _screenshotter.EncodeToPNG();
            using (var ms = new MemoryStream(png))
            {
                image = Image.FromStream(ms);
            }
            
            Clipboard.SetImage(image);
        }

        private void SaveToPNG()
        {
            var defaultName = $"{DateTime.Now:yyyy-MM-dd-hh-mm-ss}.png";
            var path = EditorUtility.SaveFilePanel("Save PNG", Application.dataPath, defaultName, "png");
            if (string.IsNullOrEmpty(path))
                return;
            
            _screenshotter.SaveToPNG(path);
        }
        
        private void OnDestroy()
        {
            _screenshotter.Dispose();
        }

        [UnityEditor.MenuItem("Tactile/Screenshot")]
        private static void ShowWindow()
        {
            var window = GetWindow<ScreenshotWindow>();
            window.titleContent = new GUIContent("Screenshot");
            window.Show();
        }
    }
}