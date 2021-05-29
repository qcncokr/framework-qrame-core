using System;
using System.Collections.Generic;

namespace Qrame.CoreFX.ExtensionMethod
{
    /// <summary>
    /// Extension methods for all kinds of (typed) enumerable data (Array, List, ...)
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Performs an action for each item in the enumerable
        /// </summary>
        /// <typeparam name="T">The enumerable data type</typeparam>
        /// <param name="@this">The data @this.</param>
        /// <param name="action">The action to be performed.</param>
        /// <example>
        /// 
        /// var @this = new[] { "1", "2", "3" };
        /// @this.ConvertList&lt;string, int&gt;().ForEach(Console.WriteLine);
        /// 
        /// </example>
        /// <remarks>This method was intended to return the passed @this to provide method chaining. Howver due to defered execution the compiler would actually never run the entire code at all.</remarks>
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var value in @this)
            {
                action(value);
            }
        }

        // var query = people.DistinctBy(p => new { p.Id, p.Name });
        // var query = people.DistinctBy(p => p.Id);
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
