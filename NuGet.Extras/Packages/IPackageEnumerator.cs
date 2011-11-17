using System;
using System.Collections.Generic;

namespace NuGet.Extras.Packages
{
    /// <summary>
    /// Interface for package enumeration logic. Allows for mocking of the PackageReference list.
    /// </summary>
    public interface IPackageEnumerator
    {
        /// <summary>
        /// Gets the package references.
        /// </summary>
        /// <param name="packageReferenceFiles">The package reference files.</param>
        /// <param name="logCount">The log count.</param>
        /// <param name="excludeVersion">if set to <c>true</c> [exclude version].</param>
        /// <returns></returns>
        IEnumerable<PackageReference> GetPackageReferences(IEnumerable<PackageReferenceFile> packageReferenceFiles, Action<string, string> logCount, bool latest = false);
    }
}