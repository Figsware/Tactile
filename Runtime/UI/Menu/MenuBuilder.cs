﻿using System;
using Tactile.Utility.Logging;
using UnityEngine;

namespace Tactile.UI.Menu
{
    public abstract class MenuBuilder : MonoBehaviour
    {
        public abstract void SetMenuObjects(MenuObject[] newMenuObjects);
    }
}