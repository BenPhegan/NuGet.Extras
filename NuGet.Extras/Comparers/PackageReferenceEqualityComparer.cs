using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NuGet.Extras.Comparers
{
    public class PackageReferenceEqualityComparer : IEqualityComparer<PackageReference>
    {
        public static readonly PackageReferenceEqualityComparer IdVersionAndAllowedVersions = new PackageReferenceEqualityComparer((x, y) =>
        {
            var _vsec = new VersionSpecEqualityComparer(x.VersionConstraint);
            if (x.VersionConstraint == null ^ y.VersionConstraint == null)
            {
                return false;
            }

            if (x.VersionConstraint == null && y.VersionConstraint == null)
            {
                return x.Id.Equals(y.Id, StringComparison.OrdinalIgnoreCase) && x.Version.Equals(y.Version);
            }

            //return x.Id.Equals(y.Id, StringComparison.OrdinalIgnoreCase) && x.Version.Equals(y.Version) && x.VersionConstraint.Equals(y.VersionConstraint);
            return x.Id.Equals(y.Id, StringComparison.OrdinalIgnoreCase) && x.Version.Equals(y.Version) && _vsec.Equals(y.VersionConstraint);

        },
            x =>
            {
                var _vsec = new VersionSpecEqualityComparer(x.VersionConstraint);
                var first = x.Id.GetHashCode() ^ x.Version.GetHashCode();
                first = x.VersionConstraint != null ? first ^ _vsec.GetHashCode() : first;
                return first;
            });

        public static readonly PackageReferenceEqualityComparer IdAndVersion = new PackageReferenceEqualityComparer((x, y) =>
        {
            if (x.Version == null ^ y.Version == null)
            {
                return false;
            }

            if (x.Version == null && y.Version == null)
            {
                return x.Id.Equals(y.Id, StringComparison.OrdinalIgnoreCase);
            }

            return x.Id.Equals(y.Id, StringComparison.OrdinalIgnoreCase) && x.Version.Equals(y.Version);
        },
                x =>
                {
                    return x.Version == null ? x.Id.GetHashCode() : x.Id.GetHashCode() ^ x.Version.GetHashCode();
                });

        public static readonly PackageReferenceEqualityComparer Id = new PackageReferenceEqualityComparer((x, y) => x.Id.Equals(y.Id, StringComparison.OrdinalIgnoreCase),
                                                                                        x => x.Id.GetHashCode());

        private readonly Func<PackageReference, PackageReference, bool> _equals;
        private readonly Func<PackageReference, int> _getHashCode;

        private PackageReferenceEqualityComparer(Func<PackageReference, PackageReference, bool> equals, Func<PackageReference, int> getHashCode)
        {
            _equals = equals;
            _getHashCode = getHashCode;
        }

        public bool Equals(PackageReference x, PackageReference y)
        {
            return _equals(x, y);
        }

        public int GetHashCode(PackageReference obj)
        {
            return _getHashCode(obj);
        }
    }
}