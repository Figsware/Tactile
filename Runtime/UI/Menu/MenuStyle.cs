using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tactile.Utility;
using Tactile.Utility.Templates;
using UnityEngine;
using UnityEngine.UI;
using Component = UnityEngine.Component;

namespace Tactile.UI.Menu
{
    [Serializable]
    public class MenuStyle : INotifyPropertyChanged
    {
        [Header("Text")] [SerializeField] private string name;
        [SerializeField] private Texture icon;
        [SerializeField, TextArea] private string description;
        [Header("Color")] [SerializeField] private UnityNullable<KeyReference<Color>> color;
        [SerializeField] private UnityNullable<KeyReference<Color>> normalColor;
        [SerializeField] private UnityNullable<KeyReference<Color>> highlightedColor;
        [SerializeField] private UnityNullable<KeyReference<Color>> pressedColor;
        [SerializeField] private UnityNullable<KeyReference<Color>> selectedColor;
        [SerializeField] private UnityNullable<KeyReference<Color>> disabledColor;

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

        public KeyReference<Color> Color => color.GetValueOrDefault();
        public KeyReference<Color> NormalColor => normalColor.GetValueOrDefault();
        public KeyReference<Color> HighlightedColor => highlightedColor.GetValueOrDefault();
        public KeyReference<Color> PressedColor => pressedColor.GetValueOrDefault();
        public KeyReference<Color> SelectedColor => selectedColor.GetValueOrDefault();
        public KeyReference<Color> DisabledColor => disabledColor.GetValueOrDefault();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetNullableTemplateItemValueField<T>(ref UnityNullable<KeyReference<T>> unityNullable,
            T? value,
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
        
        public ColorBlock SetColorBlock(ColorBlock block, Component component)
        {
            var newBlock = block;

            if (NormalColor is { } normalColor)
                newBlock.normalColor = normalColor.GetTemplateValue(component);

            if (HighlightedColor is { } highlightedColor)
                newBlock.highlightedColor = highlightedColor.GetTemplateValue(component);

            if (PressedColor is { } pressedColor)
                newBlock.pressedColor = pressedColor.GetTemplateValue(component);

            if (SelectedColor is { } selectedColor)
                newBlock.selectedColor = selectedColor.GetTemplateValue(component);

            if (DisabledColor is { } disabledColor)
                newBlock.disabledColor = disabledColor.GetTemplateValue(component);

            return newBlock;
        }
    }
}