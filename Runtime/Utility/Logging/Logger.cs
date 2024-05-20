using UnityEngine;

namespace Tactile.Utility.Logging
{
    public class Logger
    {
        public string Header;
        private readonly Object _context;

        public Logger(Object context)
        {
            _context = context;
            Header = MakeBracketHeader(context.name);
        }

        public void Info(object content)
        {
            Debug.Log(MakeLogMessage(content), _context);
        }

        public void Warning(object content)
        {
            Debug.LogWarning(MakeLogMessage(content), _context);
        }

        public void Error(object content)
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