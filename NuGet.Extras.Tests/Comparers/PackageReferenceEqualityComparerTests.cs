using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Extras.Comparers;
using NUnit.Framework;

namespace NuGet.Extras.Tests.Comparers
{
    [TestFixture]
    public class PackageReferenceEqualityComparerTests
    {
        [TestCase("Common", "1.1.1.1", null, "Common", "1.1.1.1", null, 1, 1, 1)]
        [TestCase("Common", "1.1.1.2", null, "Common", "1.1.1.1", null, 1, 2, 2)]
        [TestCase("Common", "1.1.1.1", null, "Data", "1.1.1.1", null, 2, 2, 2)]
        [TestCase("Common", "1.1.1.1", "(1.2,2.3)", "Common", "1.1.1.1", "(1.2,2.3)", 1, 1, 1)]
        [TestCase("Common", "1.1.1.1", "(1.2,2.3)", "Common", "1.1.1.1", "(1.2,2.3]", 1, 1, 2)]
        [TestCase("Common", null, null, "Common", null, null, 1, 1, 1, ExpectedException = typeof(NullReferenceException))]
        public void CheckFullSet(string id1, string v1, string vs1, string id2, string v2, string vs2, int idExpectation, int idVersionExpectation, int idVersionAndAllowedVersionExpectation)
        {
            List<PackageReference> packages = new List<PackageReference>();

            packages.Add(new PackageReference(id1, v1 == null ? null : Version.Parse(v1), vs1 == null ? null : VersionUtility.ParseVersionSpec(vs1)));
            packages.Add(new PackageReference(id2, v2 == null ? null : Version.Parse(v2), vs2 == null ? null : VersionUtility.ParseVersionSpec(vs2)));

            var idResult = packages.Distinct(PackageReferenceEqualityComparer.Id).ToList();
            var idAndVersionResult = packages.Distinct(PackageReferenceEqualityComparer.IdAndVersion).ToList();
            var idVersionAndAllowedVersionResult = packages.Distinct(PackageReferenceEqualityComparer.IdVersionAndAllowedVersions).ToList();

            Assert.IsTrue(idResult.Count == idExpectation);
            Assert.IsTrue(idAndVersionResult.Count == idVersionExpectation);
            Assert.IsTrue(idVersionAndAllowedVersionResult.Count == idVersionAndAllowedVersionExpectation);
        }

        [TestCase("Common", null, null, "Common", null, null, 1)]
        [TestCase("Common", "1.1.1.1", null, "Common", null, null, 2)]
        public void CheckIdAndVersion(string id1, string v1, VersionSpec vs1, string id2, string v2, VersionSpec vs2, int result)
        {
            List<PackageReference> packages = new List<PackageReference>();

            packages.Add(new PackageReference(id1, v1 == null ? null : Version.Parse(v1), vs1));
            packages.Add(new PackageReference(id2, v2 == null ? null : Version.Parse(v2), vs2));

            packages = packages.Distinct(PackageReferenceEqualityComparer.IdAndVersion).ToList();

            Assert.IsTrue(packages.Count == result);
        }
    }
}
