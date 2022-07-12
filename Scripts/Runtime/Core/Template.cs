using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Tactile
{

    public class Template<T> : MonoBehaviour
    {
        public Dictionary<string, T> templates = new Dictionary<string, T>();

    }
}

