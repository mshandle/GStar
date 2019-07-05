using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{

    public class AssetBundleLoader : IResource
    {
        public AssetBundleLoader()
        {

        }
        public T Load<T>(string path) where T : Object
        {
           return Resources.Load<T>(path);
        }
    }

}
