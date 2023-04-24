using UnityEngine;

namespace Tactile.Utility
{
    public class Logger
    {
        public string Header;
        private Object _context;

        public Logger(Object context)
        {
            _context = context;
            Header = MakeBracketHeader(context.name);
        }
        
        public void Log(object content)
        {
            Debug.Log(MakeLogMessage(content), _context);
        }

        public void LogWarning(object content)
        {
            Debug.LogWarning(MakeLogMessage(content), _context);
        }

        public void LogError(object content)
        {
            Debug.LogError(MakeLogMessage(content), _context);
        }

        private string MakeLogMessage(object content)
        {
            string msg = Header ?? "";
            msg += content.ToString();

            return msg;
        }

        public static string MakeBracketHeader(string text)
        {
            return $"[<color=#00ffff>{text}</color>] ";
        }
    }
}