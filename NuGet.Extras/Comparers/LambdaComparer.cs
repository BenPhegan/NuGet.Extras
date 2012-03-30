using System;
using System.Collections.Generic;

namespace NuGet.Extras.Comparers
{
    public class LambdaComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, int> _hashCodeResolver;

        public LambdaComparer(Func<T, T, bool> comparer, Func<T, int> hashCodeResolver)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (hashCodeResolver == null)
                throw new ArgumentNullException("hashCodeResolver");

            _comparer = comparer;
            _hashCodeResolver = hashCodeResolver;
        }

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hashCodeResolver(obj);
        }
    }
}
