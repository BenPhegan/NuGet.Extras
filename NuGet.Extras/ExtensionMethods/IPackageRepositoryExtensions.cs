using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGet.Extras.ExtensionMethods
{
    public static class IPackageRepositoryExtensions
    {
        /// <summary>
        /// Finds the latest package in a repository by Package Id
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="packageId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IPackage FindLatestPackage(this IPackageRepository repository, string packageId)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException("packageId");
            }
            return repository.GetPackages().Where(p => p.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase)).Where(p => p.IsLatestVersion).FirstOrDefault();			
        }

        /// <summary>
        /// Finds the latest package in a repository constrained by an Id and an IVersionSpec
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="packageId"></param>
        /// <param name="versionSpec"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IPackage FindLatestPackage(this IPackageRepository repository, string packageId, IVersionSpec versionSpec)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException("packageId");
            }
            //TODO Return the latest package between the versionsConstraint...
            return repository.GetPackages().Where((p) => versionSpec.Satisfies(p.Version) && p.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase)).Max();
        }

    }
}
