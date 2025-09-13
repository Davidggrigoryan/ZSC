using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZombieSoccer.Utitlies
{
    public static class Utilities
    {
        public static void DestroyChilds(this Transform parentTransform)
        {
            if (parentTransform.childCount > 0)
                for (int i = parentTransform.childCount - 1; i >= 0; i--)
                    UnityEngine.Object.DestroyImmediate(parentTransform.GetChild(i).gameObject);
        }        

        public static void DestroyChilds(this Transform parentTransform, params Transform[] exclude)
        {
            var tmpList = exclude.ToList();

            for (int i = parentTransform.childCount - 1; i >= 0; i--)
                if(!tmpList.Contains(parentTransform.GetChild(i)))
                    UnityEngine.Object.DestroyImmediate(parentTransform.GetChild(i).gameObject);
        }

        public static int ChildIndex(this Transform transform)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform == transform.parent.GetChild(i))
                    return i;
            }

            return -1;
        }

        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }
        ///<summary>Finds the index of the first occurrence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item) { return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i)); }
    }
}
