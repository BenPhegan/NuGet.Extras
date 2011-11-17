using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGet;

namespace NuGet.Extras.ExtensionMethods
{
    public static class IFilesSystemExtensions
    {
        public static IEnumerable<string> GetFilesRecursively(this IFileSystem fileSystem, string path, string filter)
        {
            List<string> files = new List<string>();
            files.AddRange(fileSystem.GetFiles(path, filter));
            foreach (var subDir in fileSystem.GetDirectories(path))
            {
                files.AddRange(fileSystem.GetFilesRecursively(subDir, filter));
            }
            return files;
        }
    }
}
