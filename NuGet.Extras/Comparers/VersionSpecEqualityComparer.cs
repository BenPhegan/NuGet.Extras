using System;
using NuGet;

namespace NuGet.Extras.Comparers
{
    /// <summary>
    /// Allows comparison of IVersionSpec objects
    /// </summary>
    public class VersionSpecEqualityComparer : IEquatable<IVersionSpec>
    {
        private readonly IVersionSpec _me;

        /// <summary>
        /// Provides an IVersionSpec equality comparer
        /// </summary>
        /// <param name="me"></param>
        public VersionSpecEqualityComparer(IVersionSpec me)
        {
            _me = me;
        }

        public bool Equals(IVersionSpec other)
        {
            var maxVersionsAreEqual = _me.MaxVersion != null && other.MaxVersion != null
                                 ? _me.MaxVersion.Equals(other.MaxVersion)
                                 : _me.MaxVersion == null && other.MaxVersion == null;

            var minVersionsAreEqual = _me.MinVersion != null && other.MinVersion != null
                     ? _me.MinVersion.Equals(other.MinVersion)
                     : _me.MinVersion == null && other.MinVersion == null;

            return _me.IsMaxInclusive == other.IsMaxInclusive && _me.IsMinInclusive == other.IsMinInclusive && minVersionsAreEqual && maxVersionsAreEqual;
        }

        public override int GetHashCode()
        {
            int returnHash = 0;
            if (_me.MaxVersion != null)
                returnHash ^= _me.MaxVersion.GetHashCode();
            if (_me.MinVersion != null)
                returnHash ^= _me.MinVersion.GetHashCode();

            return returnHash ^ _me.IsMaxInclusive.GetHashCode() ^ _me.IsMinInclusive.GetHashCode();
            //return _me.GetHashCode();
        }
    }
}
