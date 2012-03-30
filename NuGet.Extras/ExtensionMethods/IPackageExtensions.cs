using System.IO;

namespace NuGet.Extras.ExtensionMethods
{
    static class IPackageExtensions
    {
        public static bool IsPackageInstalled(this IPackage package, bool allowMultipleVersions, PackageManager packageManager, IFileSystem fileSystem)
        {
            var packageDir = packageManager.PathResolver.GetPackageDirectory(package.Id,package.Version);
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
