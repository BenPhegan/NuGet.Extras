using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.Extras.ExtensionMethods
{
    /// <summary>
    /// IPackageManager Extensions
    /// </summary>
    public static class IPackageManagerExtensions
    {
        /// <summary>
        /// Cleans the package folders.  Requires that the PackageManager uses an IFileSystem that has the Root set to the packages folder.
        /// </summary>
        public static void CleanPackageFolders(this IPackageManager packageManager)
        {
            var immediateDirectoryName = Path.GetFileName(packageManager.FileSystem.Root);
            if (immediateDirectoryName != null && immediateDirectoryName.Equals("packages", StringComparison.OrdinalIgnoreCase))
            {
                var directories = new ConcurrentBag<string>(packageManager.FileSystem.GetDirectories(packageManager.FileSystem.Root).ToList());
                packageManager.Logger.Log(MessageLevel.Warning, String.Format("Deleting {0} package directories from {1}.", directories.Count, packageManager.FileSystem.Root));
                Parallel.ForEach(directories, (directory) => packageManager.FileSystem.DeleteDirectory(directory, true));
            }
        }
    }
}
