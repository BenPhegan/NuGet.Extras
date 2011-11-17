using System;
using System.Collections.Generic;
using System.IO;
namespace NuGet.Extras.Repositories
{
    public interface IRepositoryManager
    {
        void CleanPackageFolders();
        IEnumerable<NuGet.PackageReferenceFile> PackageReferenceFiles { get; }
        FileInfo RepositoryConfig { get; }
    }
}
