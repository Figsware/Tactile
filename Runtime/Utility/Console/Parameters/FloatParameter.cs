using System;

namespace Tactile.Utility.Logging.Console.Parameters
{
    public class FloatParameter : Parameter<float>
    {
        public FloatParameter(string name, string description, bool isRequired) : base(name, description, isRequired)
        {
        }

        protected override void ParseParameter(string parameter, out bool isValid, out string[] autocompleteSuggestions)
        {
            isValid = float.TryParse(parameter, out _);
            autocompleteSuggestions = Array.Empty<string>();
        }

        protected override bool TryGetValue(string parameter, out float value)
        {
            return float.TryParse(parameter, out value);
        }
    }
}