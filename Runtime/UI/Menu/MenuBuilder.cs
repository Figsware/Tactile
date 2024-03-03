using System;
using Tactile.Utility;
using UnityEngine;

namespace Tactile.UI.Menu
{
    public abstract class MenuBuilder : MonoBehaviour, IMenuBuilder
    {
        public abstract void SetMenuItems(MenuItem[] newMenuItems);
    }
}