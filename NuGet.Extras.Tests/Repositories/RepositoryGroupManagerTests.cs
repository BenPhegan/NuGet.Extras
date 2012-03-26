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

        NugetMockFileSystem mfs; 

        [TestFixtureSetUp]
        public void Setup()
        {
            mfs = new NugetMockFileSystem();
            mfs.AddMockFile(new MockFileSystemInfo(ItemType.File,@"c:\files\TestSolution\packages\repositories.config",DateTime.Now,baseRepositoriesConfig),createDirectoryTree: true);
            mfs.AddMockFile(new MockFileSystemInfo(ItemType.File,@"c:\files\TestSolution\repositories.config",DateTime.Now, baseRepositoriesConfig), createDirectoryTree: false);
            mfs.AddMockDirectoryStructure(@"c:\random\empty");
        }

        [TestCase(@"c:\files\TestSolution\packages\repositories.config", 1)]
        [TestCase(@"c:\files\TestSolution\packages", 1)]
        [TestCase(@"c:\files\TestSolution", 2)]
        [TestCase(@"c:\random", 0)]
        public void ConstructParser(string repositoryConfigPath,int repositoryCount)
        {
            var repositoryManager = new RepositoryGroupManager(repositoryConfigPath, mfs);
            Assert.IsNotNull(repositoryManager);
            Assert.AreEqual(repositoryCount, repositoryManager.RepositoryManagers.Count());
        }

        [TestCase(@"c:\files\TestSolution\Project1\packages.config")]
        [TestCase(@"c:\files\TestSolution\Project2\packages.config")]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void ConstructorException(string repositoryConfigPath)
        {
            var mfs = new NugetMockFileSystem();
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
            var repositoryGroupManager = new RepositoryGroupManager(repositoryConfig, mfs);

            foreach (var repositoryManager in repositoryGroupManager.RepositoryManagers)
            {
                if (repositoryManager.RepositoryConfig.Directory != null)
                    repositoryManager.RepositoryConfig.Directory.CreateSubdirectory("Test");
            }

            repositoryGroupManager.CleanPackageFolders();

            foreach (var repositoryManager in repositoryGroupManager.RepositoryManagers)
            {
                if (repositoryManager.RepositoryConfig.Directory != null)
                    Assert.AreEqual(0, repositoryManager.RepositoryConfig.Directory.GetDirectories().Count());
            }
        }
    }
}
