using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Trinity.Encore.Framework.Core.Collections
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds a value to a ConcurrentDictionary.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="dict">The dictionary to operate on.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The element to add.</param>
        /// <exception cref="InvalidOperationException">The key already exists.</exception>
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            Contract.Requires(dict != null);
            Contract.Requires(key != null);

            if (!dict.TryAdd(key, value))
                throw new InvalidOperationException("The operation failed; the key likely exists already.");
        }

        /// <summary>
        /// Removes a value from a ConcurrentDictionary.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="dict">The dictionary to operate on.</param>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="InvalidOperationException">The key doesn't exist.</exception>
        /// <returns>The value that was removed (if any).</returns>
        public static TValue Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            Contract.Requires(dict != null);
            Contract.Requires(key != null);

            TValue value;
            if (!dict.TryRemove(key, out value))
                throw new InvalidOperationException("The operation failed; the key may not exist.");

            return value;
        }

        /// <summary>
        /// Returns the entry in this list at the given index, or the default value of the element
        /// type if the index was out of bounds.
        /// </summary>
        /// <param name="list">The list to retrieve from.</param>
        /// <param name="index">The index to try to retrieve at.</param>
        /// <returns>The value, or the default value of the element type.</returns>
        public static T TryGet<T>(this IList<T> list, int index)
        {
            Contract.Requires(list != null);
            Contract.Requires(index >= 0);

            return index >= list.Count ? default(T) : list[index];
        }

        /// <summary>
        /// Returns the entry in this dictionary at the given key, or the default value of the key
        /// if none.
        /// </summary>
        public static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            Contract.Requires(dict != null);
            Contract.Requires(key != null);

            TValue val;
            return dict.TryGetValue(key, out val) ? val : default(TValue);
        }

        public static void Swap<T>(this IList<T> self, int index1, int index2)
        {
            Contract.Requires(self != null);
            Contract.Requires(index1 != index2);
            Contract.Requires(index1 >= 0);
            Contract.Requires(index1 < self.Count);
            Contract.Requires(index2 >= 0);
            Contract.Requires(index2 < self.Count);

            var temp = self[index1];
            self[index1] = self[index2];
            self[index2] = temp;
        }

        public static void Reverse<T>(this IList<T> buffer)
        {
            Contract.Requires(buffer != null);

            var length = buffer.Count;
            for (var i = 0; i < length / 2; i++)
            {
                var temp = buffer[i];
                buffer[i] = buffer[length - i - 1];
                buffer[length - i - 1] = temp;
            }
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> self)
        {
            Contract.Requires(self != null);
            Contract.Ensures(Contract.Result<Queue<T>>() != null);

            return new Queue<T>(self);
        }

        public static Stack<T> ToStack<T>(this IEnumerable<T> self)
        {
            Contract.Requires(self != null);
            Contract.Ensures(Contract.Result<Stack<T>>() != null);

            return new Stack<T>(self);
        }

        public static C5.IPriorityQueue<T> ToPriorityQueue<T>(this IEnumerable<T> self, IComparer<T> cmp = null)
        {
            Contract.Requires(self != null);
            Contract.Ensures(Contract.Result<C5.IPriorityQueue<T>>() != null);

            var queue = new C5.IntervalHeap<T>(self.Count(), cmp);

            foreach (var item in self)
                queue.Add(item);

            return queue;
        }

        public static void AddRange<T>(this ICollection<T> col, IEnumerable<T> enumerable)
        {
            Contract.Requires(col != null);
            Contract.Requires(enumerable != null);

            foreach (var cur in enumerable)
                col.Add(cur);
        }

        public static IEnumerable<T> With<T>(this IEnumerable<T> source, Action<T> act)
        {
            Contract.Requires(source != null);
            Contract.Requires(act != null);
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            foreach (var item in source)
                act(item);

            return source; // Return the sequence directly, to avoid deferred execution in an iterator.
        }

        /// <summary>
        /// Forces execution of a deferred iterator.
        /// </summary>
        /// <typeparam name="T">The type of the items in the sequence.</typeparam>
        /// <param name="source">The sequence to process.</param>
        /// <returns>The original sequence passed to this method.</returns>
        public static IEnumerable<T> Force<T>(this IEnumerable<T> source)
        {
            Contract.Requires(source != null);

            var enumer = source.GetEnumerator();
            while (enumer.MoveNext())
            {
                // Just force execution of the iterator.
            }
        }
    }
}
