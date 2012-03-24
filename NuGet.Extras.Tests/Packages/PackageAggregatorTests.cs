using System;
using System.IO;
using System.Linq;
using NuGet.Extras.Packages;
using NuGet.Extras.Repositories;
using NUnit.Framework;
using Moq;
using NuGet;
using System.Collections.Generic;
using NuGet.Extras.Tests.TestObjects;
using ReplacementFileSystem;
using NuGet.Extras.Comparers;

namespace NuGet.Extras.Tests.Packages
{
    [TestFixture]
    public class PackageAggregatorTests
    {
        private IRepositoryManager _repositoryManager;
        private PackageAggregator _packageAggregator;

        [SetUp]
        public void SetUp()
        {
            var packageFiles = GetPackageReferenceFileList();

            var repoManagerMock = new Mock<IRepositoryManager>();
            repoManagerMock.Setup(r => r.PackageReferenceFiles).Returns(packageFiles);

            _repositoryManager = repoManagerMock.Object;
            _packageAggregator = new PackageAggregator(_repositoryManager, new PackageEnumerator());
        }


        [TestCase]
        public void ConstructorTest()
        {
            Assert.AreSame(_repositoryManager, _packageAggregator.RepositoryManager);
            Assert.IsNotNull(_packageAggregator.Packages);
        }

        [TestCase(false, 1, 2, Description="Getting latest")] // Expected value was originally '3'. test is now invalid because of version resolution
        [TestCase(true, 2, 0, Description = "Using version stated")] // Expected value was originally '3'. test is now invalid because of version resolution
        public void AggregateCount(bool getLatest, int expectedCanResolveCount, int expectedCannotResolveCount)
        {
            _packageAggregator.Compute((s, s1) => Console.WriteLine(@"{0}{1}", s, s1), PackageReferenceEqualityComparer.Id);
            Assert.AreEqual(expectedCanResolveCount, _packageAggregator.Packages.Count());
            Assert.AreEqual(expectedCannotResolveCount, _packageAggregator.PackageResolveFailures.Count());
        }

        [Test]
        public void SaveTo()
        {
            FileInfo file = _packageAggregator.Save(@".");
            Assert.IsTrue(file.Exists);
            // tidy up
            file.Delete();
        }

        [Test]
        public void SaveToTemp()
        {
            FileInfo file = _packageAggregator.Save();
            Assert.IsTrue(file.Exists);
            Assert.IsTrue(file.FullName.Contains("packages.config"));
            // tidy up
            file.Delete();
        }

        private static List<PackageReferenceFile> GetPackageReferenceFileList()
        {
            var x1 = @"<?xml version='1.0' encoding='utf-8'?>
                    <packages>
                      <package id='Package1' version='1.0' allowedVersions='[1.0, 2.0)' />
                      <package id='Package2' version='2.0' />
                    </packages>";

            var x2 = @"<?xml version='1.0' encoding='utf-8'?>
                    <packages>
                      <package id='Package1' version='2.0' allowedVersions='[1.0, 3.0)' />
                      <package id='Package2' version='2.0' />
                    </packages>";

            var mfs = new Mock<NugetMockFileSystem>();
            mfs.Setup(m => m.OpenFile("x1")).Returns(x1.AsStream());
            mfs.Setup(m => m.OpenFile("x2")).Returns(x2.AsStream());
            mfs.Setup(m => m.FileExists(It.IsAny<string>())).Returns(true);

            var mpr1 = new PackageReferenceFile(mfs.Object, "x1");
            var mpr2 = new PackageReferenceFile(mfs.Object, "x2");

            var packageFiles = new List<PackageReferenceFile>();
            packageFiles.Add(mpr1);
            packageFiles.Add(mpr2);
            return packageFiles;
        }
    }
}
