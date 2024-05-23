using System;

namespace Tactile.Utility.Console
{
    public class CommandAttribute : Attribute
    {
        public readonly string Name;
        public readonly string Description;
        public readonly bool Required;
        public readonly Parameter[] Parameters;

        public CommandAttribute(string name)
        {
            Name = name;
            Description = string.Empty;
        }

        public CommandAttribute(string name, string description, bool required, params Parameter[] parameters)
        {
            Name = name;
            Description = description;
            Required = required;
            Parameters = parameters;
        }
    }
}