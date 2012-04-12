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
            //If we are passed a null value, use a new/default VersionSpec for comparison...
            _me = me ?? new VersionSpec();
        }

        public bool Equals(IVersionSpec other)
        {
            //If we get passed a null, assume that it is a default/new VersionSpec
            if (other == null)
                other = new VersionSpec();

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
