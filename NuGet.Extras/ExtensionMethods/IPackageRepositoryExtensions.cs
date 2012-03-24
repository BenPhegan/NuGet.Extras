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
    }
}
