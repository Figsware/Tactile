using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tactile.Utility.Console.Commands
{
    [RegisterCommand]
    public class HierarchyCommand : Command
    {
        private static int MaxDepth = 3;
        public HierarchyCommand(): base("hierarchy", "Prints out information about the hierarchy", Execute)
        {
        }

        private static void Execute(ExecutionContext context)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                sb.Append($"Scene: {scene.name}");

                foreach (var go in scene.GetRootGameObjects())
                {
                    LogGameObject(sb, go, 0);
                }
            }
            
            context.ExecutingConsole.Log(sb.ToString());
        }

        private static void LogGameObject(StringBuilder sb, GameObject go, int depth)
        {
            sb.Append('\t', depth + 1);
            sb.Append(go.name);
            sb.Append('\n');

            if (depth >= MaxDepth) return;
            foreach (GameObject childGo in go.transform)
            {
                LogGameObject(sb, childGo, depth + 1);
            }
        }
    }
}