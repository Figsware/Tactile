using System;
using System.Collections.Generic;

namespace Tactile.Utility.Browser
{
    /// <summary>
    /// Represents an item within a browser.
    /// </summary>
    public abstract class BrowserItem
    {
        public readonly string Name;
        public readonly DateTime? CreatedAt;

        public BrowserItem(string name, DateTime? createdAt = null)
        {
            Name = name;
            CreatedAt = createdAt;
        }
    }

    /// <summary>
    /// A file within a browser
    /// </summary>
    public class BrowserFile : BrowserItem
    {
        public BrowserFile(string name, DateTime? createdAt = null) : base(name, createdAt)
        {
        }
    }

    /// <summary>
    /// A directory within a browser
    /// </summary>
    public class BrowserDirectory : BrowserItem
    {
        public BrowserDirectory(string name, DateTime? createdAt = null) : base(name, createdAt)
        {
        }
    }
}