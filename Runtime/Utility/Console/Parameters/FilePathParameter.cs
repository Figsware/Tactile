using System.Collections.Generic;

namespace Tactile.Utility.Console.Parameters
{
    public class FilePathParameter : Parameter<string>
    {
        private HashSet<string> AllowedExtensions = new HashSet<string>();
        
        public FilePathParameter(string name, string description, bool isRequired) : base(name, description, isRequired)
        {
        }

        public FilePathParameter(string name, string description, bool isRequired, params string[] extensions) : base(name, description, isRequired)
        {
            foreach (var extension in extensions)
            {
                AllowedExtensions.Add(extension);
            }
        }

        protected override void ParseParameter(string parameter, out bool isValid, out string[] autocompleteSuggestions)
        {
            throw new System.NotImplementedException();
        }

        protected override bool TryGetValue(string parameter, out string value)
        {
            throw new System.NotImplementedException();
        }
    }
}