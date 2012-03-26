using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGet;
using System.Text.RegularExpressions;
using System.IO;

namespace NuGet.Extras.ExtensionMethods
{
    public static class IFilesSystemExtensions
    {
        /// <summary>
        /// Gets files for a particular pattern recursively.
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="path"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetFiles(this IFileSystem fileSystem, string path, string filter, SearchOption option)
        {
            //Only build the Regex once...
            var regex = FindFilesPatternToRegex.Convert(filter);

            if (option == SearchOption.TopDirectoryOnly)
            {
                return fileSystem.GetFiles(path).Where((f) => regex.IsMatch(f));
            }
            else
            {
                return fileSystem.GetFiles(path, regex);
            }
        }

        private static IEnumerable<string> GetFiles(this IFileSystem fileSystem, string path, Regex regex)
        {
            List<string> files = new List<string>();
            files.AddRange(fileSystem.GetFiles(path).Where((f) => regex.IsMatch(f)));

            foreach (var subDir in fileSystem.GetDirectories(path))
            {
                files.AddRange(fileSystem.GetFiles(subDir, regex));
            }
            return files.Distinct();
        }

    }
}
