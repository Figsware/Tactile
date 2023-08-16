using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tactile.Utility;
using Tactile.Utility.Templates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tactile.UI
{
    [Serializable]
    public class UIAction : INotifyPropertyChanged
    {
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private Texture icon;
        [SerializeField] private bool disabled;
        [SerializeField] private UnityNullable<TemplateItem<Color>> color;
        [SerializeField] private UnityNullable<TemplateItem<Color>> normalColor;
        [SerializeField] private UnityNullable<TemplateItem<Color>> highlightedColor;
        [SerializeField] private UnityNullable<TemplateItem<Color>> pressedColor;
        [SerializeField] private UnityNullable<TemplateItem<Color>> selectedColor;
        [SerializeField] private UnityNullable<TemplateItem<Color>> disabledColor;
        [Header("Events")] public UnityEvent onSelect;

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

        public Color? Color
        {
            get => color.GetNullableTemplateItemValue();
            set => color.SetNullableTemplateItemValue(value);
        }

        public Color? NormalColor
        {
            get => normalColor.GetNullableTemplateItemValue();
            set => SetNullableTemplateItemValueField(ref normalColor, value);
        }

        public Color? HighlightedColor
        {
            get => highlightedColor.GetNullableTemplateItemValue();
            set => SetNullableTemplateItemValueField(ref highlightedColor, value);
        }

        public Color? PressedColor
        {
            get => pressedColor.GetNullableTemplateItemValue();
            set => SetNullableTemplateItemValueField(ref pressedColor, value);
        }

        public Color? SelectedColor
        {
            get => selectedColor.GetNullableTemplateItemValue();
            set => SetNullableTemplateItemValueField(ref selectedColor, value);
        }

        public Color? DisabledColor
        {
            get => disabledColor.GetNullableTemplateItemValue();
            set => SetNullableTemplateItemValueField(ref disabledColor, value);
        }

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

        protected void SetNullableTemplateItemValueField<T>(ref UnityNullable<TemplateItem<T>> unityNullable, T? value,
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

        public ColorBlock SetColorBlock(ColorBlock block)
        {
            var newBlock = block;

            if (NormalColor is { } normalColor)
                newBlock.normalColor = normalColor;

            if (HighlightedColor is { } highlightedColor)
                newBlock.highlightedColor = highlightedColor;

            if (PressedColor is { } pressedColor)
                newBlock.pressedColor = pressedColor;

            if (SelectedColor is { } selectedColor)
                newBlock.selectedColor = selectedColor;

            if (DisabledColor is { } disabledColor)
                newBlock.disabledColor = disabledColor;

            return newBlock;
        }
    }
}