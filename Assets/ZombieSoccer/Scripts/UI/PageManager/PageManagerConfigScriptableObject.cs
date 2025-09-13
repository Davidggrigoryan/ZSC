using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSoccer
{
    [CreateAssetMenu(fileName = "PageManagerConfig", menuName = "Utils/PageManagerConfig")]
    public class PageManagerConfigScriptableObject : ScriptableObject
    {
        [Tooltip("By order")]
        public List<GameObject> setupPages = new List<GameObject>();


        [Tooltip("Without order")]
        public List<GameObject> appPages = new List<GameObject>();
    }
}
