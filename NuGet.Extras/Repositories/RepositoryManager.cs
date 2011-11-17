using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics.Contracts;
using System.IO;
using ReplacementFileSystem;
using IFileSystem = ReplacementFileSystem.IFileSystem;

namespace NuGet.Extras.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        /// <summary>
        /// Gets the repository config file details.
        /// </summary>
        public FileInfo RepositoryConfig { get; private set; }
        ReplacementFileSystem.IFileSystem fileSystem;

        /// <summary>
        /// Gets the package reference files.
        /// </summary>
        public IEnumerable<PackageReferenceFile> PackageReferenceFiles { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryManager"/> class.
        /// </summary>
        /// <param name="repositoryConfig">The repository.config file to parse.</param>
        /// <param name="repositoryEnumerator">The repository enumerator.</param>
        /// <example>Can be a direct path to a repository.config file</example>
        ///   
        /// <example>Can be a path to a directory, which will recursively locate all contained repository.config files</example>
        public RepositoryManager(string repositoryConfig, IRepositoryEnumerator repositoryEnumerator, ReplacementFileSystem.IFileSystem fileSystem)
        {
            Contract.Requires(fileSystem != null);
            this.fileSystem = fileSystem;

            if (fileSystem.FileExists(repositoryConfig) && repositoryConfig.EndsWith("repositories.config"))
                RepositoryConfig = new FileInfo(repositoryConfig);
            else
                throw new ArgumentOutOfRangeException("repository");

            PackageReferenceFiles = repositoryEnumerator.GetPackageReferenceFiles(RepositoryConfig);// GetPackageReferenceFiles();
        }

        /// <summary>
        /// Cleans the package folders.
        /// </summary>
        public void CleanPackageFolders()
        {
            var root = RepositoryConfig.DirectoryName;
            if (root != null)
            {
                var directories = new ConcurrentBag<string>(fileSystem.GetDirectories(root).ToList());
                Console.WriteLine("Deleting {0} package directories from {1} folder", directories.Count, root);
                Parallel.ForEach(directories, (directory) => fileSystem.DeleteDirectory(directory, true));
            }
        }
    }
}
