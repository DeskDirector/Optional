using System.Collections;

namespace DeskDirector.Text.Json.Validation
{
    internal class ReadOnlyHashSet<T> : ISet<T>
    {
        internal ReadOnlyHashSet(HashSet<T> hashset)
        {
            HashSet = hashset ?? throw new ArgumentNullException(nameof(hashset));
        }

        internal ReadOnlyHashSet(params T[] items)
            : this(null, items)
        { }

        internal ReadOnlyHashSet(IEqualityComparer<T>? comparer, params T[]? items)
        {
            comparer ??= EqualityComparer<T>.Default;

            HashSet<T> set = HashSet = new HashSet<T>(comparer);
            if (items == null || items.Length == 0) {
                return;
            }

            foreach (T item in items) {
                if (item == null) {
                    continue;
                }

                _ = set.Add(item);
            }
        }

        public static implicit operator ReadOnlyHashSet<T>(HashSet<T>? value)
        {
            return new ReadOnlyHashSet<T>(value ?? new HashSet<T>());
        }

        private HashSet<T> HashSet { get; }

        public IEnumerator<T> GetEnumerator()
        {
            return HashSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)HashSet).GetEnumerator();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException("Readonly HashSet");
        }

        void ISet<T>.UnionWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("Readonly HashSet");
        }

        void ISet<T>.IntersectWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("Readonly HashSet");
        }

        void ISet<T>.ExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("Readonly HashSet");
        }

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("Readonly HashSet");
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return HashSet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return HashSet.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return HashSet.IsProperSupersetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return HashSet.IsProperSubsetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return HashSet.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return HashSet.SetEquals(other);
        }

        bool ISet<T>.Add(T item)
        {
            throw new NotSupportedException("Readonly HashSet");
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException("Readonly HashSet");
        }

        public bool Contains(T item)
        {
            return item != null && HashSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            HashSet.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException("Readonly HashSet");
        }

        public HashSet<T> ToSet()
        {
            return new HashSet<T>(HashSet.AsEnumerable(), HashSet.Comparer);
        }

        public int Count => HashSet.Count;

        public bool IsReadOnly => true;
    }
}