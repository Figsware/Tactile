using System;
using System.Text.RegularExpressions;

namespace Tactile.Utility.Console.Parameters
{
    public class StringParameter: Parameter<string>
    {
        private readonly Regex _pattern;

        public StringParameter(string name, string description, bool isRequired) : base(name, description, isRequired)
        {
            _pattern = null;
        }

        public StringParameter(string name, string description, bool isRequired, Regex pattern) : base(name, description, isRequired)
        {
            _pattern = pattern;
        }
        
        protected override void ParseParameter(string parameter, out bool isValid, out string[] autocompleteSuggestions)
        {
            if (_pattern != null)
            {
                isValid = _pattern.IsMatch(parameter);
            }
            else
            {
                isValid = true;
            }
            
            autocompleteSuggestions = Array.Empty<string>();
        }

        protected override bool TryGetValue(string parameter, out string value)
        {
            value = parameter;
            return true;
        }
    }
}