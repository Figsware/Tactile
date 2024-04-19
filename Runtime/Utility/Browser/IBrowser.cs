using System.Threading.Tasks;

namespace Tactile.Utility.Logging.Browser
{
    public interface IBrowser
    {
            /// <summary>
            /// The current working directory of the browser.
            /// </summary>
            public string CurrentWorkingDirectory { get; }

            /// <summary>
            /// The current items contained within the current working directory.
            /// </summary>
            public BrowserItem[] CurrentItems { get; }

            /// <summary>
            /// Goes back to the parent folder.
            /// </summary>
            public async Task GoBack()
            {
                await ChangeDirectory("..");
            }
        
            /// <summary>
            /// Changes the directory to the specified relative/absolute path. 
            /// </summary>
            /// <param name="newPath">The new path to navigate to</param>
            public Task ChangeDirectory(string newPath);
        }
}