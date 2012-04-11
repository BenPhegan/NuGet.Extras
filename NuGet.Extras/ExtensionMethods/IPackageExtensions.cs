using System.IO;

namespace NuGet.Extras.ExtensionMethods
{
    public static class IPackageExtensions
    {
        /// <summary>
        /// Determines whether [is package installed] [the specified package id].
        /// </summary>
        /// <param name="packageId">The package id.</param>
        /// <param name="version">The version.</param>
        /// <param name="allowMultipleVersions">if set to <c>true</c> [allow multiple versions].</param>
        /// <param name="packageManager">The package manager.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <returns>
        ///   <c>true</c> if [is package installed] [the specified package id]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPackageInstalled(this IPackage package, bool allowMultipleVersions, PackageManager packageManager, IFileSystem fileSystem)
        {
            var packageDir = packageManager.PathResolver.GetInstallPath(package);
            var packageFile = packageManager.PathResolver.GetPackageFileName(package.Id, package.Version);
            string packagePath = Path.Combine(packageDir, packageFile);

            if (fileSystem.FileExists(packagePath))
            {
                if (!allowMultipleVersions)
                {
                    //Need to crack the package open at this point and check the version, otherwise we just need to download it regardless
                    var zipPackage = new ZipPackage(fileSystem.OpenFile(packagePath));
                    if (zipPackage.Version == package.Version)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;

        }

    }
}
