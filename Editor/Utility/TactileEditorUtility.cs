using UnityEditor;
using UnityEngine;

namespace Tactile.Editor
{
    public static class TactileEditorUtility
    {
        [MenuItem("Tactile/Open Path/Data")]
        static void OpenDataPath()
        {
            EditorUtility.OpenWithDefaultApp(Application.dataPath);   
        }

        [MenuItem("Tactile/Open Path/Console Log")]
        static void OpenConsoleLogPath()
        {
            EditorUtility.OpenWithDefaultApp(Application.consoleLogPath);
        }
        
        [MenuItem("Tactile/Open Path/Persistent Data")]
        static void OpenPersistentDataPath()
        {
            EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
        }

        [MenuItem("Tactile/Open Path/Streaming Assets")]
        static void OpenStreamingAssetsPath()
        {
            EditorUtility.OpenWithDefaultApp(Application.streamingAssetsPath);
        }

        [MenuItem("Tactile/Open Path/Temporary Cache")]
        static void OpenTemporaryCachePath()
        {
            EditorUtility.OpenWithDefaultApp(Application.temporaryCachePath);
        }
    }
}