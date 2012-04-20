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

            var slimAggregate = GetSlimAggregateRepository(repository);
    
            return slimAggregate.GetPackages().Where(p => p.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase)).Where(p => p.IsLatestVersion).FirstOrDefault();
        }

        private static AggregateRepository GetSlimAggregateRepository(AggregateRepository repository)
        {
            //Craziness as the MachineCache and LocalPackageRepository do not support IsLatestVersion
            List<IPackageRepository> repositoryList = new List<IPackageRepository>();
            foreach (var packageRepository in repository.Repositories)
            {
                var repoType = packageRepository.GetType();
                if (repoType != typeof (LocalPackageRepository) && repoType != typeof (MachineCache))
                {
                    repositoryList.Add(packageRepository);
                }
            }
            var slimAggregate = new AggregateRepository(repositoryList);
            return slimAggregate;
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
            var slimAggregate = GetSlimAggregateRepository(repository);

            return slimAggregate.FindPackagesById(packageId).Where(p => versionSpec.Satisfies(p.Version)).OrderByDescending(p => p.Version).FirstOrDefault();
        }
    }
}
