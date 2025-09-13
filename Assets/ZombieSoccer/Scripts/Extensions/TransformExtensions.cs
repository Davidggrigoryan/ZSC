using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZombieSoccer.Extensions
{
    public static class TransformExtensions
    {
        public static List<Transform> ChildsToList(this Transform transform)
        {
            return transform.Cast<Transform>().ToList();
        }
    }
}
