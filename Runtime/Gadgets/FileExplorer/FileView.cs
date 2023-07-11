using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tactile.Gadgets.FileExplorer
{
    public class FileView : MonoBehaviour
    {
        public UnityEvent onSelect;
    
        [SerializeField] private TextMeshProUGUI nameLabel;
        [SerializeField] private RawImage fileRawImage;
        [SerializeField] private Texture fileIcon;
        [SerializeField] private Texture folderIcon;
        [SerializeField] private Texture driveIcon;

        public void SetFile(string filename, FileExplorer.FileType type)
        {
            nameLabel.text = filename;
            fileRawImage.texture = type switch
            {
                FileExplorer.FileType.File => fileIcon,
                FileExplorer.FileType.Directory => folderIcon,
                FileExplorer.FileType.Drive => driveIcon,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Select()
        {
            onSelect.Invoke();
        }
    }
}
