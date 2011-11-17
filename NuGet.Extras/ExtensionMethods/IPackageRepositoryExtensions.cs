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

            return (from p in repository.GetPackages()
                    where p.IsLatestVersion && p.Id.ToLower() == packageId.ToLower()
                    orderby p.Id, p.Version descending
                    select p).FirstOrDefault();
        }
    }
}
