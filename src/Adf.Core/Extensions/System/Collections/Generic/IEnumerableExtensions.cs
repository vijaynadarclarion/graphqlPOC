using System.Collections.ObjectModel;
using System.Linq;
using Adf.Core;

namespace System.Collections.Generic
{    
    /// <summary> 
    /// Extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// The to dictionary.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <typeparam name="TK">
        /// </typeparam>
        /// <typeparam name="TV">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        public static IDictionary<TK, TV> ToDictionary<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> list)
        {
            return list.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <summary>
        /// Partitions the IEnumerable to sets of specified size.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size)
        {
            T[] array = null;
            var count = 0;
            foreach (var item in source)
            {
                if (array == null)
                {
                    array = new T[size];
                }

                array[count] = item;
                count++;
                if (count != size)
                {
                    continue;
                }

                yield return new ReadOnlyCollection<T>(array);
                array = null;
                count = 0;
            }

            if (array == null)
            {
                yield break;
            }

            Array.Resize(ref array, count);
            yield return new ReadOnlyCollection<T>(array);
        }

        /// <summary>
        /// Gets the paged.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="pageNum">
        /// The page number.
        /// </param>
        /// <param name="pageSize">
        /// Size of the page.
        /// </param>
        /// <returns>
        /// The <see cref="PagedList{T}"/>.
        /// </returns>
        public static PagedList<T> AsPagedList<T>(this IEnumerable<T> query, int pageNum, int pageSize)
        {
            return new PagedList<T>(query, pageNum, pageSize);
        }

        /// <summary>
        /// Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type System.String, using the specified separator between each member.
        /// This is a shortcut for string.Join(...)
        /// </summary>
        /// <param name="source">A collection that contains the strings to concatenate.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
        /// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns System.String.Empty.</returns>
        public static string JoinAsString(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Concatenates the members of a collection, using the specified separator between each member.
        /// This is a shortcut for string.Join(...)
        /// </summary>
        /// <param name="source">A collection that contains the objects to concatenate.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
        /// <typeparam name="T">The type of the members of values.</typeparam>
        /// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns System.String.Empty.</returns>
        public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Filters a <see cref="IEnumerable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="source">Enumerable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the enumerable</param>
        /// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }

        /// <summary>
        /// Filters a <see cref="IEnumerable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="source">Enumerable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the enumerable</param>
        /// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }
    }
}
