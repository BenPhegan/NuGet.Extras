using NUnit.Framework;
using NuGet.Extras.Comparers;

namespace NuGet.Extras.Tests.Comparers
{
    [TestFixture]
    public class VersionSpecEqualityComparerTests
    {
        [TestCase("(1.2,2.3)", "(1.2,2.3)", true)]
        public void Test(string vs1, string vs2, bool result)
        {
            IVersionSpec ivs1 = VersionUtility.ParseVersionSpec(vs1);
            IVersionSpec ivs2 = VersionUtility.ParseVersionSpec(vs2);

            VersionSpecEqualityComparer vsec = new VersionSpecEqualityComparer(ivs1);

            Assert.IsTrue(vsec.Equals(ivs2) == result);
        }
    }
}
