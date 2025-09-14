using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections.Editor
{
    public static class SerializedDictionaryDrawer
    {
        public const string SerializedListName = "_serializedList";
        public const string KeyName = "Key";
        public const string ValueName = "Value";
        public const string LookupTableName = "_lookupTable";
        public const float TopHeaderClipHeight = 18f;
        public const float TopHeaderHeight = 18f;
        public const float SearchHeaderHeight = 18f;
        public const float KeyValueHeaderHeight = 18f;
        public const bool KeyFlag = true;
        public const bool ValueFlag = false;
        public static readonly List<int> NoEntriesList = new List<int>();
        public static readonly GUIContent DisplayTypeToggleContent = new GUIContent("Display");
        public static readonly Color BorderColor = Color.gray;
    }
}
