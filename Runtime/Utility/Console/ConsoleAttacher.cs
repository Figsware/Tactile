using System;
using UnityEngine;

namespace Tactile.Utility.Console
{
    public abstract class ConsoleAttacher : MonoBehaviour
    {
        [SerializeField] protected Console console;

        protected virtual void Awake()
        {
            AttachConsole();
        }

        private void AttachConsole()
        {
            console.OnNewConsoleText -= OnNewConsoleText;
            console.OnNewConsoleText += OnNewConsoleText;
        }

        protected void OnValidate()
        {
            AttachConsole();
        }

        protected virtual void OnNewConsoleText(string text)
        {
            
        }
    }
}