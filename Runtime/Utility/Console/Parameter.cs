using System;
using System.Text.RegularExpressions;

namespace Tactile.Utility.Console
{
    public abstract class Parameter
    {
        public readonly string Name;
        public readonly string Description;
        public readonly bool IsRequired;

        public Parameter(string name, string description, bool isRequired)
        {
            Name = name;
            Description = description;
            IsRequired = isRequired;
        }

        public abstract MatchInfo Match(string parameter);

        public abstract class MatchInfo
        {
            public readonly bool IsValid;
            public readonly string[] AutocompleteSuggestions;
            protected readonly string _parameterValue;

            public MatchInfo(string parameterValue, bool isValid, string[] autocompleteSuggestions)
            {
                _parameterValue = parameterValue;
                IsValid = isValid;
            }

            public abstract bool TryGetValue(out object value);
        }
    }

    public abstract class Parameter<T> : Parameter
    {
        public override Parameter.MatchInfo Match(string parameter)
        {
            ParseParameter(parameter, out var isValid, out var autocompleteSuggestions);
            var info = new MatchInfo(this, parameter, isValid, autocompleteSuggestions);

            return info;
        }
        
        protected abstract void ParseParameter(string parameter, out bool isValid,
            out string[] autocompleteSuggestions);

        protected abstract bool TryGetValue(string parameter, out T value);

        public new class MatchInfo : Parameter.MatchInfo
        {
            private readonly Parameter<T> _parameter;
            
            internal MatchInfo(Parameter<T> parameter, string parameterValue, bool isValid, string[] autocompleteSuggestions): base(parameterValue, isValid, autocompleteSuggestions)
            {
                _parameter = parameter;
            }

            public bool TryGetValue(out T value) => _parameter.TryGetValue(_parameterValue, out value);

            public override bool TryGetValue(out object value)
            {
                bool result = TryGetValue(out T tValue);
                value = tValue;

                return result;
            }
        }

        protected Parameter(string name, string description, bool isRequired) : base(name, description, isRequired)
        {
        }
    }
}