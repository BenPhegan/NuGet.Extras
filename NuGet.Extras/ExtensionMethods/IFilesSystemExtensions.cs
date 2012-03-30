using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NuGet.Extras.ExtensionMethods
{
    public static class IFileSystemExtensions
    {
        /// <summary>
        /// Gets files for a particular pattern recursively.
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="path"></param>
        /// <param name="filter"></param>
        /// <param name="option"> </param>
        /// <returns></returns>
        public static IEnumerable<string> GetFiles(this IFileSystem fileSystem, string path, string filter, SearchOption option)
        {
            if (option == SearchOption.TopDirectoryOnly)
            {
                return fileSystem.GetFiles(path, filter);
            }
            
            return fileSystem.GetFilesRecursive(path, filter);
        }

        private static IEnumerable<string> GetFilesRecursive(this IFileSystem fileSystem, string path, string filter)
        {
            var files = new List<string>();
            files.AddRange(fileSystem.GetFiles(path, filter));

            foreach (var subDir in fileSystem.GetDirectories(path))
            {
                files.AddRange(fileSystem.GetFiles(subDir, filter));
            }
            return files.Distinct();
        }

        public static IEnumerable<string> GetDirectories(string path)
        {
            foreach (var index in IndexOfAll(path, Path.DirectorySeparatorChar))
            {
                yield return path.Substring(0, index);
            }
            yield return path;
        }

        private static IEnumerable<int> IndexOfAll(string value, char ch)
        {
            int index = -1;
            do
            {
                index = value.IndexOf(ch, index + 1);
                if (index >= 0)
                {
                    yield return index;
                }
            }
            while (index >= 0);
        }
    }
}
