using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NuGet.Extras.Repositories
{
    public class RepositoryAssemblyResolver
    {
        List<string> assemblies = new List<string>();
        IQueryable<IPackage> packageSource;
        Dictionary<string, List<IPackage>> resolvedAssemblies = new Dictionary<string, List<IPackage>>();

        public RepositoryAssemblyResolver(List<string> assemblies, IQueryable<IPackage> packageSource)
        {
            this.assemblies = assemblies;
            this.packageSource = packageSource;

            foreach (var a in assemblies)
            {
                resolvedAssemblies.Add(a, new List<IPackage>());
            }
        }

        public Dictionary<string, List<IPackage>> ResolveAssemblies(Boolean exhaustive)
        {
            int current = 0;
            int max = packageSource.Count();

            foreach (var p in packageSource)
            {
                Console.Write("\rChecking Package {1} of {2}", p.Id, current++, max);
                var packageFiles = p.GetFiles();
                foreach (var f in packageFiles)
                {
                    FileInfo file = new FileInfo(f.Path);
                    foreach (var a in assemblies)
                    {
                        if (file.Name.ToLower() == a.ToLower())
                        {
                            resolvedAssemblies[a].Add(p);
                            //HACK Exhaustive not easy with multiple assemblies, so default to only one currently....
                            if (!exhaustive && assemblies.Count == 1)
                            {
                                return resolvedAssemblies;
                            }
                        }
                    }
                }
            }
            return resolvedAssemblies;
        }


        public void OutputPackageConfigFile(Action<string> log)
        {
            File.Delete("packages.config");
            if (!File.Exists("packages.config"))
            {
                var prf = new PackageReferenceFile(".\\packages.config");
                foreach (var pl in resolvedAssemblies)
                {
                    if (pl.Value.Count() > 0)
                    {
                        IPackage smallestPackage;
                        if (pl.Value.Count > 1)
                        {
                            smallestPackage = pl.Value.OrderBy(l => l.GetFiles().Count()).FirstOrDefault();
                            log(String.Format("{0} : Choosing : {1} - {2} to choose from.", pl.Key, smallestPackage.Id, pl.Value.Count()));
                        }
                        else
                        {
                            smallestPackage = pl.Value.First();
                        }
                        //Only add if we do not have another instance of the ID, not the id/version combo....
                        if (!prf.GetPackageReferences().Any(p => p.Id == smallestPackage.Id))
                            prf.AddEntry(smallestPackage.Id, smallestPackage.Version);
                    }
                }
            }
            else
            {
                log("Please move the existing packages.config file....");
            }
        }

    }


}
