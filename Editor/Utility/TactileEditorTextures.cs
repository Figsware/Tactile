using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Tactile.Editor
{
    public class TactileEditorTextures
    {
        private static Dictionary<string, Texture> _textures = new();

        public const string IconDirectory = "Packages/com.figsware.tactile/Icons/";
        public static readonly string BorderRadiusTopLeft = Path.Join(IconDirectory, "BorderRadius/border-radius-top-left.png");
        public static readonly string BorderRadiusTopRight = Path.Join(IconDirectory, "BorderRadius/border-radius-top-right.png");
        public static readonly string BorderRadiusBottomLeft = Path.Join(IconDirectory, "BorderRadius/border-radius-bottom-left.png");
        public static readonly string BorderRadiusBottomRight = Path.Join(IconDirectory, "BorderRadius/border-radius-bottom-right.png");
        public static readonly string BorderRadiusNoneSelected = Path.Join(IconDirectory, "BorderRadius/border-radius-none-selected.png");
        public static readonly string NullValue = Path.Join(IconDirectory, "Nullable/null.png");
        public static readonly string Value = Path.Join(IconDirectory, "Nullable/value.png");
        public static readonly string Key = Path.Join(IconDirectory, "Reference/key.png");
        public static readonly string NoKey = Path.Join(IconDirectory, "Reference/no_key.png");

        public static Texture GetTexture(string path)
        {
            if (_textures.TryGetValue(path, out var texture))
                return texture;
            
            if (!File.Exists(path)) return null;
            
            var tex = TactileUtility.TextureFromPNG(path);
            _textures[path] = tex;

            return tex;
        }
    }
}