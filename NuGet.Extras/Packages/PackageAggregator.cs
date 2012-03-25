using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using NuGet.Extras.Repositories;
using NuGet.Extras.PackageReferences;
using NuGet.Extras.Comparers;

namespace NuGet.Extras.Packages
{
    /// <summary>
    /// Manages the saving of the aggregated packages.config file.
    /// </summary>
    public class PackageAggregator : IDisposable
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IPackageEnumerator _packageEnumerator;
        private readonly bool _autoDelete;
        private IEnumerable<PackageReference> _packages;
        private IEnumerable<PackageReference> _packagesResolveFailures;
        private FileInfo _fileInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageAggregator"/> class.
        /// </summary>
        /// <param name="repositoryManager">The repository manager.</param>
        /// <param name="packageEnumerator">The package enumerator.</param>
        /// <param name="autoDelete">if set to <c>true</c> [auto delete].</param>
        public PackageAggregator(IRepositoryManager repositoryManager, IPackageEnumerator packageEnumerator, bool autoDelete = false)
        {
            _repositoryManager = repositoryManager;
            _packageEnumerator = packageEnumerator;
            _autoDelete = autoDelete;
            _packages = new List<PackageReference>();
        }

        /// <summary>
        /// Gets the repository manager.
        /// </summary>
        public IRepositoryManager RepositoryManager
        {
            get { return _repositoryManager; }
        }

        /// <summary>
        /// Gets the packages.
        /// </summary>
        public IEnumerable<PackageReference> Packages
        {
            get { return _packages; }
        }

        /// <summary>
        /// Gets the Package resolution failures.
        /// </summary>
        public IEnumerable<PackageReference> PackageResolveFailures
        {
            get { return _packagesResolveFailures; }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_fileInfo != null)
                if (_fileInfo.Exists && _autoDelete)
                    _fileInfo.Delete();
        }

        #endregion

        /// <summary>
        /// Computes the list of PackageReference objects.
        /// </summary>
        /// <param name="logCount">The log count.</param>
        /// <param name="excludeVersion">if set to <c>true</c> [exclude version].</param>
        public void Compute(Action<string, string> logCount, PackageReferenceEqualityComparer comparer, IPackageReferenceSetResolver resolver)
        {
            _packages = _packageEnumerator.GetPackageReferences(_repositoryManager.PackageReferenceFiles, logCount, comparer);

            //TODO not sure this is correct...
            var returnLists = resolver.Resolve(_packages);
            _packages = returnLists.Item1;
            _packagesResolveFailures = returnLists.Item2;
        }

        /// <summary>
        /// Saves the packages to a packages.config file in the specified directory.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public FileInfo Save(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            XDocument xml = CreatePackagesConfigXml(_packages);
            string fileName = Path.Combine(directory, "packages.config");
            xml.Save(fileName);
            _fileInfo = new FileInfo(fileName);
            return _fileInfo;
        }

        /// <summary>
        /// Saves the packages to a packages.config file in the temp directory.
        /// </summary>
        /// <returns></returns>
        public FileInfo Save()
        {
            return Save(Path.GetTempPath() + Guid.NewGuid().ToString());
        }

        private XDocument CreatePackagesConfigXml(IEnumerable<PackageReference> packages)
        {
            var doc = new XDocument();
            var packagesElement = new XElement("packages");

            foreach (PackageReference p in packages)
            {
                var packageXml = new XElement("package");
                packageXml.SetAttributeValue("id", p.Id);
                packageXml.SetAttributeValue("version", p.Version);
                if (p.VersionConstraint != null)
                    packageXml.SetAttributeValue("allowedVersions", p.VersionConstraint.ToString());
                packagesElement.Add(packageXml);
            }

            doc.Add(packagesElement);
            return doc;
        }
    }
}