using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using NuGet.Extras.Tests.TestObjects;
using NuGet.Extras.ExtensionMethods;

namespace NuGet.Extras.Tests.Extensions
{
    [TestFixture]
    public class AggregateRepositoryExtensionsTests
    {
        [Test]
        public void CanCreateNewAggregateRepositoryExcludingLocalRepositories()
        {
            var mfs = new Mock<MockFileSystem>() {CallBase = true};
            var pr = new DefaultPackagePathResolver(mfs.Object);
            var mc = MachineCache.Default;
            var l = new LocalPackageRepository(pr, mfs.Object);

            var r1 = new DataServicePackageRepository(new Uri(@"http://nuget.org"));
            var r2 = new DataServicePackageRepository(new Uri(@"http://beta.nuget.org"));

            var ar = new AggregateRepository(new List<IPackageRepository>() {mc, l, r1, r2});
            Assert.AreEqual(2, ar.GetRemoteOnlyAggregateRepository().Repositories.Count());
            Assert.AreEqual(typeof(DataServicePackageRepository), ar.GetRemoteOnlyAggregateRepository().Repositories.ToArray()[0].GetType());
            Assert.AreEqual(typeof(DataServicePackageRepository), ar.GetRemoteOnlyAggregateRepository().Repositories.ToArray()[1].GetType());
        }

    }
}
