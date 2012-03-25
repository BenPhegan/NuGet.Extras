using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics.Contracts;
using NuGet.Extras.ExtensionMethods;
using IFileSystem = ReplacementFileSystem.IFileSystem;

namespace NuGet.Extras.Repositories
{
    public class RepositoryGroupManager
    {
        public IEnumerable<RepositoryManager> RepositoryManagers { get; private set; }
        ReplacementFileSystem.IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryGroupManager"/> class.  Provides nested operations over RepositoryManager instances.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <example>Can be a direct path to a repository.config file</example>
        ///   
        /// <example>Can be a path to a directory, which will recursively locate all contained repository.config files</example>
        public RepositoryGroupManager(string repository, ReplacementFileSystem.IFileSystem fileSystem)
        {
            Contract.Requires(fileSystem != null);
            this.fileSystem = fileSystem;
            
            if (fileSystem.DirectoryExists(repository))
            {
                // we're dealing with a directory
                //TODO Does this by default do a recursive???
                RepositoryManagers = new ConcurrentBag<RepositoryManager>(fileSystem.GetFiles(repository, "repositories.config").Select(file => new RepositoryManager(file, new RepositoryEnumerator(fileSystem), fileSystem)).ToList());
            }
            else if (fileSystem.FileExists(repository) && Path.GetFileName(repository) == "repositories.config")
                RepositoryManagers = new ConcurrentBag<RepositoryManager>(new List<RepositoryManager> { new RepositoryManager(repository, new RepositoryEnumerator(fileSystem), fileSystem) });
            else
                throw new ArgumentOutOfRangeException("repository");
        }

        /// <summary>
        /// Cleans the package folders.
        /// </summary>
        public void CleanPackageFolders()
        {
            Parallel.ForEach(RepositoryManagers, (repositoryManager) => repositoryManager.CleanPackageFolders());
        }

        /// <summary>
        /// Installs the packages.
        /// </summary>
        public void InstallPackages()
        {

        }
    }
}
