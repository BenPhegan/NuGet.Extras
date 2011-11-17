using System;
using NuGet;

namespace NuGet.Extras.Comparers
{
    public class VersionSpecEqualityComparer : IEquatable<IVersionSpec>
    {
        private IVersionSpec _me;

        public VersionSpecEqualityComparer(IVersionSpec me)
        {
            _me = me;
        }

        public bool Equals(IVersionSpec other)
        {
            return _me.IsMaxInclusive == other.IsMaxInclusive && _me.IsMinInclusive == other.IsMinInclusive && _me.MaxVersion.Equals(other.MaxVersion) && _me.MinVersion.Equals(other.MinVersion);
        }

        public override int GetHashCode()
        {
            return _me.IsMaxInclusive.GetHashCode() ^ _me.IsMinInclusive.GetHashCode() ^ _me.MaxVersion.GetHashCode() ^ _me.MinVersion.GetHashCode();
            //return _me.GetHashCode();
        }
    }
}
