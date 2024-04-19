using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Tactile.Utility.Logging.Folder
{
    [Serializable]
    public class Folder<TDirectory, TFile>
    {
        public Directory root = new();
        
        [Serializable]
        public class Directory
        {
            public TDirectory directory;
            public List<int> children;
            public List<TFile> files;
            public List<TDirectory> directories;
        }
    }
}