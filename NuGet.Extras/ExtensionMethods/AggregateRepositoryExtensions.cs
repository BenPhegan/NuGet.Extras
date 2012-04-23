using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGet.Extras.ExtensionMethods
{
    public static class AggregateRepositoryExtensions
    {

        /// <summary>
        /// Finds the latest package in a repository by Package Id
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="packageId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IPackage FindLatestPackage(this AggregateRepository repository, string packageId)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException("packageId");
            }

            var remoteOnlyAggregateRepository = repository.GetRemoteOnlyAggregateRepository();
    
            return remoteOnlyAggregateRepository.GetPackages().Where(p => p.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase)).Where(p => p.IsLatestVersion).FirstOrDefault();
        }

        /// <summary>
        /// Returns a an AggregateRepository minus any LocalPackageRepositories or MachineCache repositories.  Useful if you want to use a command that will not work across these types.
        /// Snappy name, I know.
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public static AggregateRepository GetRemoteOnlyAggregateRepository(this AggregateRepository repository)
        {
            //Craziness as the MachineCache and LocalPackageRepository do not support IsLatestVersion
            var excluded = new List<Type> {typeof (LocalPackageRepository), typeof (MachineCache)};
            return new AggregateRepository(repository.Repositories.Where(r => !excluded.Contains(r.GetType())));
        }

        /// <summary>
        /// Finds the latest package in a repository constrained by an Id and an IVersionSpec
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="packageId"></param>
        /// <param name="versionSpec"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IPackage FindLatestPackage(this AggregateRepository repository, string packageId, IVersionSpec versionSpec)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException("packageId");
            }
            //TODO Return the latest package between the versionsConstraint...
            var remoteOnlyAggregateRepository = repository.GetRemoteOnlyAggregateRepository();

            return remoteOnlyAggregateRepository.FindPackagesById(packageId).Where(p => versionSpec.Satisfies(p.Version)).OrderByDescending(p => p.Version).FirstOrDefault();
        }
    }
}
