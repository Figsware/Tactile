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
        public static string IconLibraryUrl =
            "https://raw.githubusercontent.com/Templarian/MaterialDesign-SVG/master/meta.json";

        public static string BaseApiUrl = "https://dev.materialdesignicons.com/api/";
        
        
        private string search;
        private Vector2 scrollPos = Vector2.zero;
        private IconMetadata[] cache;
        private IconMetadata[] searchCache;
        private static HttpClient client = new HttpClient();

        [MenuItem("Tactile/Material Design Icons")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MDIcons));
        }

        private async void Awake()
        {
            cache = await DownloadIcons();
            searchCache = cache;
            Debug.Log("Downloaded icons!");
        }

        private void OnGUI()
        {
            GUILayout.Label("Search");
            
            EditorGUI.BeginChangeCheck();
            search = GUILayout.TextField(search);
            var changed = EditorGUI.EndChangeCheck();
            
            if (changed)
            {
                searchCache = cache.Where(i => i.name.Contains(search)).ToArray();
            }

            if (searchCache != null)
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                for (int i = 0; i < Mathf.Min(searchCache.Length, 15); i++)
                {
                    GUILayout.Label(searchCache[i].name);
                }
                GUILayout.EndScrollView();
            }
        }

        public async Task<IconMetadata[]> DownloadIcons()
        {
            var response = await client.GetAsync(IconLibraryUrl);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Debug.LogError("Unable to download Material Design icons!");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var icons = JsonConvert.DeserializeObject<IconMetadata[]>(content);
            Debug.Log(icons.Length);
            
            return icons;
        }

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

            public async Task Download()
            {
                // var iconUrl = Path.Join(BaseApiUrl,);
            }
        }
    }
}