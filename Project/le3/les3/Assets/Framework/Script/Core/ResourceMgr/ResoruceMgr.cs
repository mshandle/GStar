using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Framework
{
    public class ResoruceMgr : BaseComponentTemplate<ResoruceMgr>
    {
        private IResource resLoader = null;
        public override bool Init(AppConfig config)
        {
            switch (config.loadMode)
            {
                case ResourceLoadMode.ASSETDATA:
                    resLoader = new LocalLoader();
                    break;
                case ResourceLoadMode.ASSETBUNDLE:
                    resLoader = new AssetBundleLoader();
                    break;
              
            }
            return base.Init(config);
        }

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return resLoader.Load<T>(path);
        }
    }

}

