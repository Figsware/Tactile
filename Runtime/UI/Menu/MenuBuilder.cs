using System;
using Tactile.Utility;
using UnityEngine;

namespace Tactile.UI.Menu
{
    /// <summary>
    /// Represents a builder that can build a menu of requested MenuObjects. 
    /// </summary>
    public abstract class MenuBuilder : MonoBehaviour
    {
        /// <summary>
        /// Builds the specified set of MenuObjects with the builder.
        /// </summary>
        /// <param name="newMenuObjects">The MenuObjects to build</param>
        public abstract void SetMenuObjects(MenuObject[] newMenuObjects);
    }
}