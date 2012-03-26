using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics.Contracts;

namespace NuGet.Extras.Repositories
{
    public class RepositoryEnumerator : IRepositoryEnumerator
    {
        IFileSystem fileSystem;

        public RepositoryEnumerator(IFileSystem fileSystem)
        {
            Contract.Requires(fileSystem != null);
            this.fileSystem = fileSystem;
        }


        /// <summary>
        /// Gets the package reference files.
        /// </summary>
        /// <param name="repositoryConfig">The repository config.</param>
        /// <returns></returns>
        public IEnumerable<PackageReferenceFile> GetPackageReferenceFiles(FileInfo repositoryConfig)
        {
            var packageConfigs = new List<PackageReferenceFile>();

            //TODO OpenFile probably needs to be a little simpler....
            XDocument doc = XDocument.Load(fileSystem.OpenFile(repositoryConfig.FullName));
            XElement repositories = doc.Element("repositories");
            if (repositories != null)
            {
                foreach (var repository in repositories.Descendants("repository"))
                {
                    var packageconfig = (string)repository.Attribute("path") ?? "";
                    if (!string.IsNullOrEmpty(packageconfig))
                    {
                        var fullpath = Path.GetFullPath(repositoryConfig.Directory + "..\\" + packageconfig);
                        packageConfigs.Add(new PackageReferenceFile(fullpath));
                    }
                }
            }
            return packageConfigs;
        }
    }
}
