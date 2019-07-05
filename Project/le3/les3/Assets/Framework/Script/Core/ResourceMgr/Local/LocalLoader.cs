using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Framework
{
    public class LocalLoader : IResource
    {
        private static string prePath = "Assets/App/Data/";
        public T Load<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<T>(prePath + path);
#else
            return null;
#endif
        }
    }

}

