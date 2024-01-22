using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

namespace Tactile.Editor.Gadgets
{
    public class MDIcons : EditorWindow
    {
        public static string IconMetadataUrl =
            "https://raw.githubusercontent.com/Templarian/MaterialDesign-SVG/master/meta.json";

        public static string IconDownloadBaseUrl =
            "https://raw.githubusercontent.com/Templarian/MaterialDesign-SVG/master/svg/";

        public static string BaseApiUrl = "https://dev.materialdesignicons.com/api/";

        public static string IconBrowserUrl = "https://pictogrammers.com/library/mdi/";


        private static IconMetadata[] _metadataCache;
        private static readonly HttpClient Client = new HttpClient();

        private IconMetadata[] _searchCache;
        private string search;
        private int pageIndex = 0;
        private int resultsPerPage = 50;
        private bool fillWithWhite = true;
        private Vector2 scrollPos = Vector2.zero;


        [MenuItem("Tactile/Material Design Icons")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MDIcons));
        }

        private async void Awake()
        {
            _metadataCache ??= await DownloadIcons();
            _searchCache = _metadataCache;
            Debug.Log("Downloaded icons!");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Icon Browser"))
                Application.OpenURL(IconBrowserUrl);
            
            GUILayout.Label("Search");

            EditorGUI.BeginChangeCheck();
            search = GUILayout.TextField(search);
            var changed = EditorGUI.EndChangeCheck();

            fillWithWhite = GUILayout.Toggle(fillWithWhite, "Fill SVG with White");

            if (changed && _metadataCache != null)
            {
                _searchCache = _metadataCache.Where(i => string.IsNullOrEmpty(search) || i.name.Contains(search))
                    .ToArray();
            }

            if (_searchCache != null)
            {
                GUILayout.BeginVertical("box");
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                var start = Mathf.Min(_searchCache.Length, resultsPerPage * pageIndex);
                var end = Mathf.Min(_searchCache.Length, start + resultsPerPage);
                
                for (int i = start; i < end; i++)
                {
                    var bgRow = i % 2 == 1;
                    if (bgRow)
                    {
                        GUILayout.BeginVertical("box");
                    }
                    
                    var metadata = _searchCache[i];
                    GUILayout.BeginHorizontal();
                    
                    if (bgRow)
                        GUILayout.Space(-4);
                    
                    GUILayout.Label(metadata.name);
                    if (GUILayout.Button("Download", GUILayout.Width(75)))
                        _searchCache[i].Download(fillWithWhite);
                    
                    if (bgRow)
                        GUILayout.Space(-4);
                    
                    GUILayout.EndHorizontal();
                    
                    if (bgRow)
                        GUILayout.EndVertical();
                }

                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }

            GUILayout.BeginHorizontal();

            var numPages = _searchCache != null ? Mathf.CeilToInt(_searchCache.Length / (float)resultsPerPage) : 0;
            var maxPageIndex = Mathf.Max(numPages - 1, 0);

            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("<<"))
                pageIndex = 0;

            if (GUILayout.Button("<"))
                pageIndex = Mathf.Max(0, pageIndex - 1);

            pageIndex = Mathf.Clamp(EditorGUILayout.IntField(pageIndex + 1, GUILayout.Width(50)) - 1,
                0, maxPageIndex);
            GUILayout.Label($"out of {numPages}");

            if (GUILayout.Button(">"))
                pageIndex = Mathf.Min(maxPageIndex, pageIndex + 1);

            if (GUILayout.Button(">>"))
                pageIndex = maxPageIndex;
            
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
        }

        public async Task<IconMetadata[]> DownloadIcons()
        {
            var response = await Client.GetAsync(IconMetadataUrl);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Debug.LogError("Unable to download Material Design icons!");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var icons = JsonConvert.DeserializeObject<IconMetadata[]>(content);

            return icons;
        }

        [Serializable]
        public class IconMetadata
        {
            public string id;
            public string baseIconId;
            public string name;
            public string codepoint;
            public string[] aliases;
            public string[] tags;
            public string author;
            public string version;

            public async void Download(bool fillWithWhite)
            {
                var path = EditorUtility.SaveFilePanelInProject($"Download {name}", name, "svg",
                    $"Choose a destination for the \"{name}\" icon.");
                var iconUrl = IconDownloadBaseUrl + $"{name}.svg";
                await using (var stream = await Client.GetStreamAsync(iconUrl))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var svg = await reader.ReadToEndAsync();
                        if (fillWithWhite)
                            svg = svg.Replace("<svg", @"<svg fill=""#FFFFFF""");
                        await File.WriteAllTextAsync(path, svg);
                    }
                }
                
                // Reload the assets
                AssetDatabase.Refresh();
            }
        }
    }
}