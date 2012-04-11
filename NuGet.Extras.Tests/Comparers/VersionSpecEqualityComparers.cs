using System;
using NUnit.Framework;
using NuGet.Extras.Comparers;

namespace NuGet.Extras.Tests.Comparers
{
    [TestFixture]
    public class VersionSpecEqualityComparerTests
    {
        [TestCase("(1.2,2.3)", "(1.2,2.3)", true)]
        [TestCase("(1.2,2.3)", "(1.2,2.4)", false)]
        [TestCase("(1.2,2.3)", "", false)]
        [TestCase("", "(1.2,2.4)", false)]
        public void Test(string vs1, string vs2, bool result)
        {
            IVersionSpec ivs1 = String.IsNullOrEmpty(vs1) ? new VersionSpec() : VersionUtility.ParseVersionSpec(vs1);
            IVersionSpec ivs2 = String.IsNullOrEmpty(vs2) ? new VersionSpec() : VersionUtility.ParseVersionSpec(vs2);

            var vsec = new VersionSpecEqualityComparer(ivs1);

            Assert.IsTrue(vsec.Equals(ivs2) == result);
        }
    }
}
