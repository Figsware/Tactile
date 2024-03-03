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
        [SerializeField, TextArea] private string description;
        [Header("Icons")] [SerializeField] private Texture icon;
        [Header("Color")] [SerializeField] private UnityNullable<Template<Color>.Reference> color;
        [SerializeField] private UnityNullable<Template<Color>.Reference> normalColor;
        [SerializeField] private UnityNullable<Template<Color>.Reference> highlightedColor;
        [SerializeField] private UnityNullable<Template<Color>.Reference> pressedColor;
        [SerializeField] private UnityNullable<Template<Color>.Reference> selectedColor;
        [SerializeField] private UnityNullable<Template<Color>.Reference> disabledColor;

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

        public Template<Color>.Reference Color => color.GetValueOrDefault();
        public Template<Color>.Reference NormalColor => normalColor.GetValueOrDefault();
        public Template<Color>.Reference HighlightedColor => highlightedColor.GetValueOrDefault();
        public Template<Color>.Reference PressedColor => pressedColor.GetValueOrDefault();
        public Template<Color>.Reference SelectedColor => selectedColor.GetValueOrDefault();
        public Template<Color>.Reference DisabledColor => disabledColor.GetValueOrDefault();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetNullableTemplateItemValueField<T>(ref UnityNullable<Template<T>.Reference> unityNullable,
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
    }
}