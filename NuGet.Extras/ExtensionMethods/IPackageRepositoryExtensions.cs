using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGet.Extras.ExtensionMethods
{
    static class IPackageRepositoryExtensions
    {
        public static IPackage FindLatestPackage(this IPackageRepository repository, string packageId)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException("packageId");
            }
			
			return repository.GetPackages().Where((p) => p.IsLatestVersion && p.Id.Equals(packageId,StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public static IPackage FindLatestPackage(this IPackageRepository repository, string packageId, string versionConstraint)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException("packageId");
            }
            //TODO Return the latest package between the versionsConstraint...
            return repository.GetPackages().Where((p) => p.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

    }
}
