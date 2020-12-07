using System;
using System.IO;
using System.Reflection;

namespace SoundMastery.DataAccess.Common
{
    public static class EmbeddedResource
    {
        private const string DefaultProjectNamespace = "SoundMastery.DataAccess";

        public static string GetAsString(string localName, string localPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileName = $"{DefaultProjectNamespace}.{localPath}.{localName}";
            var resource = assembly.GetManifestResourceStream(fileName);
            if (resource == null)
            {
                throw new ArgumentException($"Could not find resource with name {fileName}");
            }

            using var reader = new StreamReader(resource);
            return reader.ReadToEnd();
        }
    }
}
