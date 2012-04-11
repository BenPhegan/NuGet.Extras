using System.IO;
using Moq;
using NuGet.Extras.ExtensionMethods;
using NuGet.Extras.Tests.TestObjects;
using NUnit.Framework;

namespace NuGet.Extras.Tests.Extensions
{
    [TestFixture]
    public class IPackageExtensionsTests
    {
        [TestCase("Assembly.Common", "2.1.9", "2.1.9", false, Description = "On Disk is same version", Result = true)]
        [TestCase("Assembly.Common", "2.1.8", "2.1.9", false, Description = "On disk is older", Result = false)]
        [TestCase("Assembly.Common", "2.1.9", "2.1.8", false, Description = "On disk is newer", Result = false)]
        [TestCase("Assembly.Common", "", "2.1.8", false, Description = "No on disk results in false", Result = false)]
        public bool CanDetermineVersionlessPackageIsInstalled(string id, string onDiskVersion, string packageVersion, bool allowMultipleVersions)
        {
            var mfs = new Mock<MockFileSystem>() { CallBase = true };
            mfs.Setup(m => m.Root).Returns(@"c:\packages");

            var pr = new DefaultPackagePathResolver(mfs.Object, allowMultipleVersions);

            var testPackage = new DataServicePackage()
                                  {
                                      Version = packageVersion,
                                      Id = id
                                  };

            var filePackage = new DataServicePackage()
                                  {
                                      Version = onDiskVersion,
                                      Id = id
                                  };

            IPackage zipPackage = null;
            if (!string.IsNullOrEmpty(onDiskVersion))
            {
                string baseLocation = pr.GetInstallPath(filePackage);
                string fileName = pr.GetPackageFileName(filePackage.Id, SemanticVersion.Parse(filePackage.Version));
                string filePath = Path.Combine(baseLocation, fileName);
                zipPackage = PackageUtility.GetZipPackage(filePackage.Id, filePackage.Version);
                mfs.Setup(m => m.FileExists(filePath)).Returns(true);
                mfs.Setup(m => m.OpenFile(filePath)).Returns(zipPackage.GetStream());
                mfs.Object.AddFile(filePath, zipPackage.GetStream());
            }

            var pm = new PackageManager(new MockPackageRepository(), pr, mfs.Object);
            var test = testPackage.IsPackageInstalled(allowMultipleVersions, pm, mfs.Object);
            return test;
        }

}
}
