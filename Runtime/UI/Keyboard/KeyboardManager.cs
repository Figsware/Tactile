using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Logger = Tactile.Utility.Logger;

namespace Tactile.UI.Menu.Keyboard
{
    public class KeyboardManager : MonoBehaviour
    {
        public UnityEvent<KeyCode> onKeyDown;
        public UnityEvent<KeyCode> onKeyUp;

        public delegate void ModifierChangedHandler(Modifier modifier, bool newValue);

        public event ModifierChangedHandler onModifierChange; 
        public float repeatDelay = 1f;
        public float repeatInterval = 0.5f;
        public GameObject target;
        private Coroutine _pressedKeyCoroutine = null;

        public enum Modifier
        {
            Default,
            Shift,
            CapsLock,
            NumLock,
            ScrollLock
        }

        public bool this[KeyCode key]
        {
            get => _keys[key];
            set => SetKeyDown(key, value);
        }

        public bool this[Modifier modifier]
        {
            get => _modifiers[modifier];
            set => SetModifier(modifier, value);
        }

        public static readonly KeyCode[] Keys = (KeyCode[])Enum.GetValues(typeof(KeyCode));

        public static readonly Modifier[] Modifiers = (Modifier[])Enum.GetValues(typeof(Modifier));

        private bool IsTextUpperCase => (this[Modifier.CapsLock] && !this[Modifier.Shift]) || (!this[Modifier.CapsLock] && this[Modifier.Shift]);

        private Logger _logger;
        private Dictionary<KeyCode, bool> _keys;
        private Dictionary<Modifier, bool> _modifiers;

        private void Awake()
        {
            _logger = new(this);
            InitializeKeys();
        }

        public void SetKeyDown(KeyCode key, bool pressed)
        {
            if (_keys[key] == pressed)
            {
                // Don't do anything if the state of they key hasn't changed.
                return;
            }
            
            _keys[key] = pressed;
            
            if (pressed)
            {
                onKeyDown.Invoke(key);
                
                ModifyTargetWithKey(key);
                
                if (key is >= KeyCode.A and <= KeyCode.Z)
                {
                    SetModifier(Modifier.Shift, false);
                } 
                else if (key is KeyCode.LeftShift or KeyCode.RightShift)
                {
                    ToggleModifier(Modifier.Shift);
                } 
                else if (key == KeyCode.CapsLock)
                {
                    ToggleModifier(Modifier.CapsLock);
                }
                
                StartRepeatKey(key);
            }
            else
            {
                onKeyUp.Invoke(key);
                StopRepeatKey();
            }
            
            _logger.Log($"Key: {key}, Pressed: {pressed}");
        }

        public void ToggleModifier(Modifier modifier)
        {
            SetModifier(modifier, !_modifiers[modifier]);
        }

        public void SetModifier(Modifier modifier, bool modifierEnabled)
        {
            if (_modifiers[modifier] == modifierEnabled)
                return;

            _modifiers[modifier] = modifierEnabled;
            onModifierChange?.Invoke(modifier, modifierEnabled);
        }

        /// <summary>
        /// Pastes text from the clipboard into the keyboard.
        /// </summary>
        public void Paste()
        {
            var pasteText = GUIUtility.systemCopyBuffer;
            _logger.Log("Pasting: " + pasteText);
            InsertText(pasteText);
        }

        /// <summary>
        /// Inserts text into the keyboard.
        /// </summary>
        /// <param name="text"></param>
        public void InsertText(string text)
        {
            ModifyTarget(_ => text);
        }

        private void StartRepeatKey(KeyCode key)
        {
            StopRepeatKey();
            _pressedKeyCoroutine = StartCoroutine(RepeatKeyCoroutine(key));
        }

        private void StopRepeatKey()
        {
            if (_pressedKeyCoroutine != null)
            {
                StopCoroutine(_pressedKeyCoroutine);
            }
        }

        private IEnumerator RepeatKeyCoroutine(KeyCode key)
        {
            if (repeatInterval > 0)
            {
                yield return new WaitForSeconds(repeatDelay);
                _logger.Log($"Repeating key: {key}");
                while (true)
                {
                    ModifyTargetWithKey(key);
                    yield return new WaitForSeconds(repeatInterval);
                }
            }
        }

        private void ModifyTargetWithKey(KeyCode key)
        {
            ModifyTarget(prev =>
            {
                string text = prev;
                    
                // A-Z
                if (key is >= KeyCode.A and <= KeyCode.Z)
                {
                    text += IsTextUpperCase ? key.ToString() : key.ToString().ToLower();
                } 
                else if (key == KeyCode.Space)
                {
                    text += " ";
                }
                else if (key == KeyCode.Backspace)
                {
                    text = text.Length > 0 ? text.Substring(0, text.Length - 1) : text;
                }

                return text;
            });
        }

        private void ModifyTarget(Func<string, string> modifier)
        {
            if (!target)
                return;
            
            if (target.GetComponent<InputField>() is { } inputField)
            {
                inputField.text = modifier(inputField.text);
                inputField.caretPosition = inputField.text.Length;
            } 
            else if (target.GetComponent<TMP_InputField>() is { } tmpInputField)
            {
                tmpInputField.text = modifier(tmpInputField.text);
                tmpInputField.stringPosition = tmpInputField.text.Length;
            }
        }

        private void InitializeKeys()
        {
            _keys = new Dictionary<KeyCode, bool>();
            foreach (var key in (KeyCode[]) Enum.GetValues(typeof(KeyCode)))
            {
                _keys.TryAdd(key, false);
            }

            _modifiers = new Dictionary<Modifier, bool>();
            foreach (var modifier in (Modifier[])Enum.GetValues(typeof(Modifier)))
            {
                _modifiers.TryAdd(modifier, false);
            }
        }
    }
}