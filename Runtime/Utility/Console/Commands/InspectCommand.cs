using System.Text;
using Tactile.Utility.Console.Parameters;
using UnityEngine;

namespace Tactile.Utility.Console.Commands
{
    [RegisterCommand]
    public class InspectCommand : Command<string>
    {
        public InspectCommand() : base("inspect", "Inspects information about a GameObject", Execute,
            new StringParameter("path", "The path of the GameObject to inspect", true))
        {
            
        }
        
        private static void Execute(ExecutionContext context)
        {
            var path = context.Arg1;
            var go = GameObject.Find(path);

            if (!go)
            {
                context.ExecutingConsole.LogError("Could not find GameObject with the path: " + path);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Name: {go.name}");
            sb.AppendLine($"Active Self: {go.activeSelf}");
            sb.AppendLine($"Active Hierarchy: {go.activeInHierarchy}");
            sb.AppendLine($"Tag: {go.tag}");
            sb.AppendLine($"Is Static: {go.isStatic}");
            
            context.ExecutingConsole.Log(sb.ToString());
        }
    }
}