using System;
using System.Linq;
using NUnit.Framework;
using NuGet.Extras.Repositories;
using NuGet.Extras.Tests.TestObjects;
using System.IO;
using ReplacementFileSystem;
using IFileSystem = ReplacementFileSystem.IFileSystem;
using ItemType = ReplacementFileSystem.ItemType;

namespace NuGet.Extras.Tests.Repositories
{
    [TestFixture]
    public class RepositoryGroupManagerTests
    {
        #region Global Constructor Tests
        string baseRepositoriesConfig = @"<?xml version='1.0' encoding='utf-8'?>
                                            <repositories>
                                              <repository path='..\Project1\packages.config' />
                                              <repository path='..\Project2\packages.config' />
                                              <repository path='..\Project3\packages.config' />
                                            </repositories>";

        [TestCase(@"c:\files\TestSolution\packages\repositories.config", true, 0, 1)]
        [TestCase(@"c:\files\TestSolution\packages\", false, 1, 1)]
        [TestCase(@"c:\files\TestSolution\", false, 2, 2)]
        public void ConstructParser(string repositoryConfigPath, Boolean file, int additionalFileCount, int repositoryCount)
        {
            var mfs = new MockFileSystem();
            if (file)
            {
                mfs.Info.Add(MockFileSystemInfo.CreateFileObject(repositoryConfigPath, baseRepositoriesConfig));
            }
            else
            {
                mfs.CreateDirectory(repositoryConfigPath);
                var test = mfs.DirectoryExists(repositoryConfigPath);
            }


            for (int x = 0; x < additionalFileCount; x++)
            {
                mfs.Info.Add(MockFileSystemInfo.CreateFileObject(String.Concat(repositoryConfigPath, x, Path.DirectorySeparatorChar, "repositories.config"), baseRepositoriesConfig));
            }

            var repositoryManager = new RepositoryGroupManager(repositoryConfigPath, mfs);
            Assert.IsNotNull(repositoryManager);
            Assert.AreEqual(repositoryCount, repositoryManager.RepositoryManagers.Count());
        }

        [TestCase(@"c:\files\TestSolution\Project1\packages.config")]
        [TestCase(@"c:\files\TestSolution\Project2\packages.config")]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void ConstructorException(string repositoryConfigPath)
        {
            var mfs = new MockFileSystem();
            mfs.Info.Add(MockFileSystemInfo.CreateFileObject(repositoryConfigPath));
            new RepositoryGroupManager(repositoryConfigPath, mfs);
        }

        #endregion

        private readonly string _repositoryConfigPath;
        private RepositoryGroupManager _repositoryManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryManagerTests"/> class.
        /// </summary>
        /// <param name="repositoryConfigPath">The repository config path.</param>
        //public RepositoryGroupManagerTests(string repositoryConfigPath)
        //{
        //    _repositoryConfigPath = repositoryConfigPath;
        //}

        [TestCase(@"c:\files\TestSolution\packages\repositories.config")]
        public void CanCleanPackageFolders(string repositoryConfig)
        {
            //[TestFixture(@"..\..\files\TestSolution\packages\repositories.config")]
            //[TestFixture(@"..\..\files\TestSolution\packages")]
            //[TestFixture(@"..\..\files\TestSolution")]

            var mfs = new MockFileSystem();
            mfs.AddMockFile(MockFileSystemInfo.CreateFileObject(repositoryConfig, baseRepositoriesConfig));

            foreach (var repositoryManager in _repositoryManager.RepositoryManagers)
            {
                if (repositoryManager.RepositoryConfig.Directory != null)
                    repositoryManager.RepositoryConfig.Directory.CreateSubdirectory("Test");
            }

            _repositoryManager.CleanPackageFolders();

            foreach (var repositoryManager in _repositoryManager.RepositoryManagers)
            {
                if (repositoryManager.RepositoryConfig.Directory != null)
                    Assert.AreEqual(0, repositoryManager.RepositoryConfig.Directory.GetDirectories().Count());
            }
        }
    }
}
