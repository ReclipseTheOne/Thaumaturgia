using System;

namespace Thaumaturgia.Utils {
    public class ResourceLocation {
        public string NameSpace { get; }
        public string Path { get; }

        public ResourceLocation(string nameSpace, string path) {
            if (string.IsNullOrEmpty(nameSpace))
                throw new ArgumentException("NameSpace cannot be null or empty.", nameof(nameSpace));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));

            NameSpace = nameSpace;
            Path = path;
        }

        public ResourceLocation AsChild(string path) {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));

            return new ResourceLocation(NameSpace, Path + "/" + path);
        }

        public static ResourceLocation FromDefaultNamespace(string path) {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));

            return new ResourceLocation("thaumaturgia", path);
        }

        public static ResourceLocation TryParse(string path) {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));

            string[] parts = path.Split(':');
            if (parts.Length == 2) {
                return new ResourceLocation(parts[0], parts[1]);
            } else {
                throw new ArgumentException("Invalid ResourceLocation format. Expected 'namespace:path'.", nameof(path));
            }
        }
    }
}