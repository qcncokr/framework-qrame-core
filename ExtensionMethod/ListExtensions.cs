using System;
using System.Collections.Generic;
using System.Linq;

namespace Qrame.CoreFX.ExtensionMethod
{
    /// <summary>
    /// Extension methods for all kind of Lists implementing the IList&lt;T&gt; interface
    /// </summary>
    public static class ListExtensions
    {

        /// <summary>
        /// Inserts an item uniquely to to a @this and returns a value whether the item was inserted or not.
        /// </summary>
        /// <typeparam name="T">The generic @this item type.</typeparam>
        /// <param name="@this">The @this to be inserted into.</param>
        /// <param name="index">The index to insert the item at.</param>
        /// <param name="item">The item to be added.</param>
        /// <returns>Indicates whether the item was inserted or not</returns>
        public static bool InsertUnqiue<T>(this IList<T> @this, int index, T item)
        {
            if (@this.Contains(item) == false)
            {
                @this.Insert(index, item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Inserts a range of @this uniquely to a @this starting at a given index and returns the amount of @this inserted.
        /// </summary>
        /// <typeparam name="T">The generic @this item type.</typeparam>
        /// <param name="@this">The @this to be inserted into.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="@this">The @this to be inserted.</param>
        /// <returns>The amount if @this that were inserted.</returns>
        public static int InsertRangeUnique<T>(this IList<T> @this, int startIndex, IEnumerable<T> target)
        {
            var index = startIndex;
            foreach (var item in target)
            {
                if (@this.InsertUnqiue(startIndex, item)) index++;
            }
            return (index - startIndex);
        }

        /// <summary>
        /// Return the index of the first matching item or -1.
        /// </summary>
        /// <typeparam name="T">제너릭 타입</typeparam>
        /// <param name="@this">The @this.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns>The item index</returns>
        public static int IndexOf<T>(this IList<T> @this, Func<T, bool> comparison)
        {
            for (var i = 0; i < @this.Count; i++)
            {
                if (comparison(@this[i])) return i;
            }
            return -1;
        }

        public static IList<T> AddRange<T>(this IList<T> @this, IList<T> itemsToAdd)
        {
            if ((@this != null) && (itemsToAdd != null))
            {
                foreach (T local in itemsToAdd)
                {
                    @this.Add(local);
                }
            }
            return @this;
        }

        public static bool IsNullOrEmpty<T>(IList<T> @this)
        {
            return ((@this == null) || (@this.Count<T>() == 0));
        }

    }
}
