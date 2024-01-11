using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tactile.Utility;
using Tactile.Utility.Templates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Component = UnityEngine.Component;

namespace Tactile.UI
{
    [Serializable]
    public class UIAction : INotifyPropertyChanged, ISerializationCallbackReceiver
    {
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private Texture icon;
        [SerializeField] private bool disabled;
        [SerializeField] private string radioGroupId;
        [SerializeField] private bool toggleable;
        [SerializeField] private UnityNullable<Template<Color>.Reference> color;
        [SerializeField] private UnityNullable<Template<Color>.Reference> normalColor;
        [SerializeField] private UnityNullable<Template<Color>.Reference> highlightedColor;
        [SerializeField] private UnityNullable<Template<Color>.Reference> pressedColor;
        [SerializeField] private UnityNullable<Template<Color>.Reference> selectedColor;
        [SerializeField] private UnityNullable<Template<Color>.Reference> disabledColor;
        [Header("Events")] public UnityEvent onSelect;
        [Header("Events")] public UnityEvent onDeselect;

        private bool selected = false;

        private static Dictionary<string, UIAction> RadioGroupSelections = new();

        public string Name
        {
            get => name;
            set => SetField(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetField(ref description, value);
        }

        public Texture Icon
        {
            get => icon;
            set => SetField(ref icon, value);
        }

        public Template<Color>.Reference Color => color;

        public Template<Color>.Reference NormalColor => normalColor;
        public Template<Color>.Reference HighlightedColor => highlightedColor;
        public Template<Color>.Reference PressedColor => pressedColor;
        public Template<Color>.Reference SelectedColor => selectedColor;
        public Template<Color>.Reference DisabledColor => disabledColor;

        public bool Disabled
        {
            get => disabled;
            set => SetField(ref disabled, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetNullableTemplateItemValueField<T>(ref UnityNullable<Template<T>.Reference> unityNullable, T? value,
            [CallerMemberName] string propertyName = null)
            where T : struct
        {
            unityNullable.SetNullableTemplateItemValue(value);
            OnPropertyChanged(propertyName);
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void SetActionForGameObject(GameObject go)
        {
            var configurators = go.GetComponents<BaseActionConfigurator>();
            foreach (var configurator in configurators)
            {
                configurator.SetUIAction(this);
            }
        }

        public ColorBlock SetColorBlock(ColorBlock block, Component component)
        {
            var newBlock = block;

            if (NormalColor is { } normalColor)
                newBlock.normalColor = normalColor.GetValue(component);

            if (HighlightedColor is { } highlightedColor)
                newBlock.highlightedColor = highlightedColor.GetValue(component);

            if (PressedColor is { } pressedColor)
                newBlock.pressedColor = pressedColor.GetValue(component);

            if (SelectedColor is { } selectedColor)
                newBlock.selectedColor = selectedColor.GetValue(component);

            if (DisabledColor is { } disabledColor)
                newBlock.disabledColor = disabledColor.GetValue(component);

            return newBlock;
        }

        public void Invoke()
        {
            if (!string.IsNullOrEmpty(radioGroupId))
            {
                var otherIsSelected = RadioGroupSelections.ContainsKey(radioGroupId);
                if (!otherIsSelected || RadioGroupSelections[radioGroupId] != this)
                {
                    if (otherIsSelected)
                    {
                        RadioGroupSelections[radioGroupId].Deselect();
                    }

                    RadioGroupSelections[radioGroupId] = this;
                    Select();
                }
            }
            else if (toggleable)
            {
                if (selected)
                {
                    Deselect();
                }
                else
                {
                    Select();
                }
            }
            else
            {
                Select();
            }
        }

        private void Select()
        {
            Debug.Log(name + " selected!");
            onSelect.Invoke();
            selected = toggleable || !string.IsNullOrEmpty(radioGroupId);
        }

        private void Deselect()
        {
            Debug.Log(name + " deselected!");
            onDeselect.Invoke();
            selected = false;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }
    }
}