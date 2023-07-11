using System.IO;
using Tactile;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tactile.Gadgets.FileExplorer
{
    public class FileExplorer : MonoBehaviour
    {
        public UnityEvent<string> onFileSelected;

        [SerializeField] private TextMeshProUGUI currentDirectoryLabel;
        [SerializeField] private LayoutGroup fileViewGroup;
        [SerializeField] private FileView fileViewPrefab;
        [SerializeField] private string homeDirectory;

        public string CurrentWorkingDirectory
        {
            get => _currentWorkingDirectory;
            set => SetWorkingDirectory(value);
        }

        private string _currentWorkingDirectory;

        public enum FileType
        {
            File,
            Directory,
            Drive
        }

        private void Awake()
        {
            _currentWorkingDirectory = homeDirectory;
        }

        private void Start()
        {
            GoToHomeDirectory();
        }

        public void GoToHomeDirectory()
        {
            SetWorkingDirectory(homeDirectory);
        }

        public void GoBack()
        {
            var newPath = Path.GetFullPath(Path.Combine(_currentWorkingDirectory, ".."));
        
            // If the path doesn't change after going back a directory (which can happen if you're at the root of a drive),
            // just set the new path to the empty string.
            if (newPath == _currentWorkingDirectory)
                newPath = string.Empty;
        
            SetWorkingDirectory(newPath);
        }

        public void SetWorkingDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                fileViewGroup.gameObject.DestroyAllChildren();
                var drives = Directory.GetLogicalDrives();

                foreach (var drive in drives)
                {
                    CreateFileView(drive, FileType.Drive);
                }
            
                _currentWorkingDirectory = string.Empty;
                currentDirectoryLabel.text = string.Empty;
            } 
            else if (Directory.Exists(path))
            {
                fileViewGroup.gameObject.DestroyAllChildren();
            
                var files = Directory.GetFiles(path);
                var directories = Directory.GetDirectories(path);

                foreach (var directory in directories)
                {
                    CreateFileView(directory, FileType.Directory);
                }
            
                foreach (var file in files)
                {
                    CreateFileView(file, FileType.File);
                }
            
                _currentWorkingDirectory = path;
                currentDirectoryLabel.text = path;
            }
        }
    
        private FileView CreateFileView(string path, FileType fileType)
        {
            var filename = fileType switch
            {
                FileType.File => Path.GetFileName(path),
                FileType.Directory => Path.GetFileName(path),
                _ => path
            };
            var view = Instantiate(fileViewPrefab, fileViewGroup.transform);
            view.SetFile(filename, fileType);
        
            switch (fileType)
            {
                case FileType.File:
                    view.onSelect.AddListener(() => onFileSelected.Invoke(path));
                    break;
            
                case FileType.Drive:
                case FileType.Directory:
                    view.onSelect.AddListener(() => SetWorkingDirectory(path));
                    break;
            }

            return view;
        }
    }
}
